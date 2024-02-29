using ServerKaLoop.GameModels.Handlers;
using ServerKaLoop.Rooms.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Rooms.Interfaces
{
    public interface IRoomManager
    {
        BaseRoom Lobby { get; set; }
        RoomHandler RoomDb { get; set; }

        BaseRoom FindRoom(string id);
        BaseRoom CreateRoom();
        BaseRoom CreateRoom(string quizzId, int time = 10, int MaxPlayers = 50);

        bool AddRoom(BaseRoom room);
        bool AddRoom(string id, BaseRoom room);

        bool RemoveRoom(string id);

    

    }
}
