
using ServerKaLoop.Applications.Messaging.Constants;

public interface IPlayer 
{
    public string SessionId {get;set;}
    public string Name {get;set;}

    void SetDisconnect(bool value);

    bool SendMessage(string message);

    bool SendMessage<T>(WsMessage<T> message);


    void OnDisconnect();

    UserInfo GetUserInfo();

}