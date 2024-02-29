using System.Net;
using System.Net.Sockets;
using GameDatabase.Database.Handlers;
using NetCoreServer;
using Newtonsoft.Json.Serialization;
using ServerKaLoop.Logging;
using ServerKaLoop.Rooms.Interfaces;

namespace ServerKaLoop.Applications.Interfaces
{
    public class WsGameServer: WsServer, IWsGameServer
    {
        private int _port;
        public readonly IPlayerManager PlayerManager;
        private readonly IGameLogger _logger;
        private readonly MongoDb _mongoDb;
        public readonly IRoomManager RoomManager;

        public WsGameServer(IPAddress address, int port,IPlayerManager playerManager,
            IGameLogger logger, MongoDb mongo,IRoomManager roomManager) : base(address,port)
        {
            _port = port;
            PlayerManager=playerManager;
            _logger = logger;
            _mongoDb = mongo;
            RoomManager = roomManager;
        }
        protected override TcpSession CreateSession()
        {
            _logger.Info("New Session Connected");
            var player = new Player(this,_mongoDb.GetDatabase());

            PlayerManager.AddPlayer(player);

            return player;
        }

        protected override void OnDisconnected(TcpSession session)
        {
            _logger.Info("Session Disconnected");
            var player = PlayerManager.FindPlayer(session.Id.ToString());
            if(player != null)
            {
                PlayerManager.RemovePlayer(player.SessionId);
            }
            base.OnDisconnected(session);
        }
        public void SendAll (string mess) {
            this.MulticastText(mess);
        }

        public void StartServer(){
            //todo logic before start server
            if(this.Start())
            {
                _logger.Info($"Start Server at port{_port} ");
                return;
            }
        
        }
        protected override void OnError(SocketError error)
        {
            _logger.Error("Error: " + error);
            base.OnError(error);
        }
        public void StopServer(){
            //todo logic before stop server
            this.Stop();
            _logger.Print($"Server Stopped ");

        }

        public void RestartServer(){
            //todo logic before restart server
            this.Restart();
            _logger.Print($"Server Restarted ");

        }

    }
}
