using System.Runtime.Intrinsics.Arm;
using System.Text;
using GameDatabase.Database.Handlers;
using GameDatabase.Database.Interfaces;
using MongoDB.Driver;
using NetCoreServer;
using ServerKaLoop.Applications.Interfaces;
using ServerKaLoop.Applications.Messaging.Constants;
using ServerKaLoop.GameModels;
using ServerKaLoop.GameModels.Handlers;
using ServerKaLoop.Logging;
using ServerKaLoop.Rooms.Handlers;
using ServerKaLoop.Rooms.Interfaces;

public class Player: WsSession,IPlayer
{
    public bool IsDisconnected {get;set;}
    public string Name {get;set;} 
    public string SessionId {get;set;}

    private readonly IGameLogger _logger;

    private UserHandler UserDb { get; set; }

    private AccountHandler AccountDb { get; set; }
    private QuizzesHandler QuizzDb { get; set; }

    public bool canAnswer = false;

    private User UserInfo {  get; set;}

    private GameRoom CurrentRoom { get; set; }

    



    public Player(WsServer server, IMongoDatabase database) : base(server)
    {
        SessionId = this.Id.ToString();
        IsDisconnected = false;
        Name = "Guest";
        _logger = new GameLogger();
        UserDb = new UserHandler(database);
        AccountDb = new AccountHandler(database);
        QuizzDb = new QuizzesHandler(database);

    }

    public override void OnWsConnected(HttpRequest request)
    {
        //todo login on player connected

        _logger.Info("Player Connected");
        IsDisconnected = false;      
    }
    public override void OnWsDisconnected()
    {
        OnDisconnect();
        base.OnWsDisconnected();
    }
    public override void OnWsReceived(byte[] buffer, long offset, long size)
    {
        var mess = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
        try
        {
            var wsMess = GameHelper.ParseStruct<WsMessage<object>>(mess);
            switch(wsMess.Tags){
                case WsTags.Invalid:
                    break;
                case WsTags.Login:
                    var loginData = GameHelper.ParseStruct<LoginData>(wsMess.Data.ToString());
                    UserInfo = UserDb.FindByUserName(loginData.Username);

                    if (UserInfo != null )
                    {
                        var hassPass = GameHelper.hashPasswordMD5(loginData.Password);
                        if (hassPass == UserInfo.Password)
                        {
                            //todo move UsertoLobby
                            var messInfo = new WsMessage<UserInfo>(WsTags.UserInfo, this.GetUserInfo());
                            this.PlayerJoinLobby();
                            return;
                        }
                    }
                    var invalidMess = new WsMessage<string>(WsTags.Invalid, "Invalid username or password");
                    this.SendMessage(GameHelper.ParseString(invalidMess));
                    break;
                case WsTags.Register:
                    var registerData = GameHelper.ParseStruct<RegisterData>(wsMess.Data.ToString());

                    if (UserInfo != null)
                    {
                        var messsage = new WsMessage<string>(WsTags.Invalid, "You logined");
                        this.SendMessage(GameHelper.ParseString(messsage));
                        return;
                    }
                    var check = UserDb.FindByUserName(registerData.Username);
                    if (check != null)
                    {
                        var messsage = new WsMessage<string>(WsTags.Invalid, "Username already exists");
                        this.SendMessage(GameHelper.ParseString(messsage));
                        return;
                    }
                  
                    var userRegister = new User(registerData.Username, registerData.Password, registerData.DisplayName);
                    UserInfo = UserDb.Create(userRegister);
                    if (UserInfo != null)
                    {
                        //todo move UsertoLobby
                        this.PlayerJoinLobby();
                        return;
                    }
                    break;
                case WsTags.RoomInfo:
                    break;
                case WsTags.JoinRoom:
                    var joinRoomData = GameHelper.ParseStruct<JoinRoomData>(wsMess.Data.ToString());
                    var checkAcc = AccountDb.FindByUserToken(joinRoomData.Email);
                    if (checkAcc != null)
                    {
                        var messsage = new WsMessage<string>(WsTags.Invalid, "Dang Nhap Thanh Cong");

                        this.SendMessage(GameHelper.ParseString(messsage));
                        UserInfo = new User("", "", joinRoomData.DisplayName);
                        UserInfo.IsAdmin = true;
                        this.PlayerJoinLobby(joinRoomData.RoomCode);
                        return;
                    }

                    UserInfo = new User("","", joinRoomData.DisplayName);
                    this.PlayerJoinLobby(joinRoomData.RoomCode);          
                    break;
                case WsTags.CreateRoom:
                    var createRoomData = GameHelper.ParseStruct<CreateRoomData>(wsMess.Data.ToString());
                    OnCreateRoom(createRoomData);
                    break;
                case WsTags.StartGame:
                    var StartGameData = GameHelper.ParseStruct<CreateRoomData>(wsMess.Data.ToString());
                    if (UserInfo.IsAdmin)
                    {
                        var room = (GameRoom)((WsGameServer)Server).RoomManager.FindRoom(StartGameData.RoomCode);
                        if(room._gameStarted)
                        {
                            NotifyData messageNotify = new NotifyData { type = "info", title = "Info", text = "This room is already started" };
                            this.SendMessage(GameHelper.ParseString(new WsMessage<NotifyData>(WsTags.Notify, messageNotify)));
                            return;
                        }
                        if (room == null)
                        {
                            NotifyData messageNotify = new NotifyData { type = "info", title = "Info", text = "This room does not exist" };
                            this.SendMessage(GameHelper.ParseString(new WsMessage<NotifyData>(WsTags.Notify, messageNotify)));
                            return;
                        }                    
                        room.StartGame();
                        _logger.Info("Start Game: " + StartGameData.RoomCode);                   
                    }
                    break;
                case WsTags.Answer:
                    if (this.canAnswer)
                    {
                        var answerData = GameHelper.ParseStruct<AnswerData>(wsMess.Data.ToString());
                        OnAnswer(answerData);
                    }
                    else
                    {
                        NotifyData messageNotify = new NotifyData { type = "warning", title = "Waring" , text = "You can't answer now, please wait!" };
                        this.SendMessage(GameHelper.ParseString(new WsMessage<NotifyData>(WsTags.Notify, messageNotify)));
                    }
                    break;
                default:
                    break;
            }
        }
        catch(Exception e)
        {
            _logger.Error("OnWsReceived: " ,e);
        }
        //((WsGameServer) Server).SendAll($"{this.SessionId} send messages: {mess}");

    }
    void OnAnswer(AnswerData answerData)
    {
        bool isCorrect = CurrentRoom.Answer(answerData);
        canAnswer = false;
        if (isCorrect)
        {
            long tenSecondsTicks = TimeSpan.FromSeconds(11).Ticks;
            UserInfo.Score += (CurrentRoom.lastQuestionTime + tenSecondsTicks) - DateTime.Now.Ticks;
            _logger.Info($"Player: {UserInfo.DisplayName},Score: {UserInfo.Score}");              
        }
        var mess = new WsMessage<UserInfo>(WsTags.UserInfo, this.GetUserInfo());
        this.SendMessage(GameHelper.ParseString(mess));
        this.SendMessage(GameHelper.ParseString(new WsMessage<string>(WsTags.FlipCard, "Flip card!")));

    }
    private void OnCreateRoom(CreateRoomData createRoomData)
    {

        if (UserInfo.IsAdmin)
        {
            if(((WsGameServer)Server).RoomManager.FindRoom(createRoomData.RoomCode) != null)
            {
                NotifyData messageNotify = new NotifyData { type = "warning", title = "Waring", text = "This room already exists" };
                this.SendMessage(GameHelper.ParseString(new WsMessage<NotifyData>(WsTags.Notify, messageNotify)));
                return;
            }         
            var room = (GameRoom)((WsGameServer)Server).RoomManager.CreateRoom(createRoomData.QuizzesId, 10, 20);
            room.quizzHandler = this.QuizzDb;
            if (room != null && room.JoinRoom(this))
            {
                ((WsGameServer)Server).RoomManager.AddRoom(createRoomData.RoomCode, room);
                _logger.Info("Create Room: " + createRoomData.RoomCode);
                var lobby = ((WsGameServer)Server).RoomManager.Lobby;
                lobby.ExitRoom(this);
                CurrentRoom = room;
                room.roomHandler = ((WsGameServer)Server).RoomManager.RoomDb;
                room.roomModel = room.roomHandler.FindByRoomCode(createRoomData.RoomCode);
                room.UpdateOnlineRoom();
            }
        }
    }
    private void PlayerJoinLobby()
    {
        var lobby = ((WsGameServer)Server).RoomManager.Lobby;
        lobby.JoinRoom(this);
    }
    private void PlayerJoinLobby(string id)
    {
        var room = (GameRoom)((WsGameServer)Server).RoomManager.FindRoom(id);

        if (room == null  && CurrentRoom == null)
        {
            var lobby = ((WsGameServer)Server).RoomManager.Lobby;
            lobby.JoinRoom(this);
            _logger.Info($"{UserInfo.DisplayName} joined lobby");
            return;
        }
        if (CurrentRoom == null)
        {
            CurrentRoom = room;
            room.JoinRoom(this);
            _logger.Info($"{UserInfo.DisplayName} joined room: {room.Id}");
            return;
        }
        if(CurrentRoom!=null)
        {
            CurrentRoom.ExitRoom(this);
            CurrentRoom = room;
            room.JoinRoom(this);
            _logger.Info($"{UserInfo.DisplayName} joined room: {room.Id}");
            return;

        }

    }
    public void SetDisconnect(bool value){
        this.IsDisconnected = value;
    }
  
    public void OnDisconnect(){

        if(UserInfo != null)
        {
            UserInfo.IsOnline = false;
            UserDb.Update(UserInfo.Id, UserInfo);
        }
        ((WsGameServer)Server).RoomManager.Lobby.ExitRoom(this);
        if (CurrentRoom != null)
            CurrentRoom.ExitRoom(this);
            
        _logger.Warning("Player Disconnected",null);

    }
    
    public UserInfo GetUserInfo()
    {
        if(UserInfo != null)
        {
            return new()
            {
                UserId = UserInfo.Id, 
                DisplayName = UserInfo.DisplayName,
                Amount = UserInfo.Amount,
                Score = UserInfo.Score,
                Avatar = UserInfo.Avatar

            };
        }
        return new UserInfo();
    }
    public bool SendMessage(string message)
    {
        return this.SendTextAsync(message);
    }
    public bool SendMessage<T>(WsMessage<T> message)
    {
        var mess = GameHelper.ParseString(message);
        return this.SendMessage(mess);
    }
}