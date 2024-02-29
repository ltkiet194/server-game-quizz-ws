using MongoDB.Driver;
using ServerKaLoop.GameModels;
using ServerKaLoop.GameModels.Handlers;
using ServerKaLoop.Rooms.Constants;
using ServerKaLoop.Rooms.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Rooms.Handlers
{
    public class RoomManager : IRoomManager
    {
        public BaseRoom Lobby { get ; set; }

        private ConcurrentDictionary<string, BaseRoom> Rooms { get; set; }

        public RoomHandler RoomDb { get; set; }
        public RoomManager(IMongoDatabase database) {
            RoomDb = new RoomHandler(database);
            Rooms = new ConcurrentDictionary<string, BaseRoom>();
            Lobby = new BaseRoom(RoomType.Lobby);
                  
        }

        public BaseRoom CreateRoom(string quizzId, int time = 10, int MaxPlayers = 50)
        {
            var newRoom = new GameRoom(quizzId, time, MaxPlayers);
            Rooms.TryAdd(newRoom.Id, newRoom);
            return newRoom;
        }
        public BaseRoom FindRoom(string id)
        {
            return Rooms.FirstOrDefault(x => x.Key.Equals(id)).Value;
        }
 
        public bool RemoveRoom(string id)
        {
            var oldRoom = FindRoom(id);
            if(oldRoom != null)
            {
                Rooms.TryRemove(id, out var room);
                return room != null;
            }
            return false;
        }

        public bool AddRoom(string id,BaseRoom room)
        {
            if (Rooms.TryAdd(id, room))
            {
                return true;
            }
            return false;
        }

        public bool AddRoom(BaseRoom room)
        {
            throw new NotImplementedException();
        }

        public BaseRoom CreateRoom()
        {
            throw new NotImplementedException();
        }
    }
}
