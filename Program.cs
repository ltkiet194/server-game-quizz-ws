// See https://aka.ms/new-console-template for more information

using System.Net;
using GameDatabase.Database.Handlers;
using ServerKaLoop.Applications.Interfaces;
using ServerKaLoop.Logging;
using ServerKaLoop.Rooms.Handlers;
using ServerKaLoop.Rooms.Interfaces;
class Program
{
    static void Main(string[] args)
    {
        IGameLogger logger = new GameLogger();
        var mongodb = new MongoDb();
        IPlayerManager playerManager = new PlayerManager(logger);
        IRoomManager roomManager = new RoomManager(mongodb.GetDatabase());
        var wsServer = new WsGameServer(IPAddress.Any, 8765 , playerManager , logger,mongodb, roomManager);   
        wsServer.StartServer();
        logger.Print("Game Server Start!");
        for(;;){
            var type =Console.ReadLine();
            if(type == "stop"){
                wsServer.StopServer();
                logger.Print("Game Server Stop!");
                break;
            }
            if(type == "restart"){
                logger.Print("Game Server Restart!");

                wsServer.RestartServer();
            }                
        }      
    }
}
