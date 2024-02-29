using GameDatabase.Database.Handlers;
using GameDatabase.Database.Interfaces;
using MongoDB.Driver;
using ServerKaLoop.GameModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.GameModels.Handlers
{
    public class RoomHandler : IDbHandler<RoomModel>
    {
        private readonly IGameDB<RoomModel> _roomDB;
        public RoomHandler(IMongoDatabase database)
        {
            _roomDB = new MongoHandler<RoomModel>(database);
        }
        public RoomModel Create(RoomModel item)
        {
            item = _roomDB.Create(item);
            return item;
        }

        public RoomModel Find(string id)
        {
            var filter = Builders<RoomModel>.Filter.Eq(i => i.Id, id);
            return _roomDB.Get(filter);
        }
        public RoomModel FindByRoomCode(string id)
        {
            var filter = Builders<RoomModel>.Filter.Eq(i => i.RoomCode  , id);
            return _roomDB.Get(filter);
        }
        public List<RoomModel> FindAll()
        {
            return _roomDB.GetAll();
        }

        public void Remove(string id)
        {
            var filter = Builders<RoomModel>.Filter.Eq(i => i.Id, id);
            _roomDB.Remove(filter);
        }

        public RoomModel Update(string id, RoomModel item)
        {
            var filter = Builders<RoomModel>.Filter.Eq(i => i.Id, id);
            var updater = Builders<RoomModel>.Update
                .Set(i => i.RoomCode, item.RoomCode);
            return item;
        
        }
        public RoomModel UpdateByRoomCode(string roomCode, RoomModel item)
        {
            var filter = Builders<RoomModel>.Filter.Eq(i => i.RoomCode, roomCode);
            var updater = Builders<RoomModel>.Update
                .Set(i => i.RoomCode, item.RoomCode)
                .Set(i => i.UpdatedAt, DateTime.Now)
                .Set(i => i.RoomName, item.RoomName)
                .Set(i => i.isStart, item.isStart)
                .Set(i => i.isEnd, item.isEnd)
                .Set(i => i.isFull, item.isFull)
                .Set(i => i.Online, item.Online);
            _roomDB.Update(filter, updater);
            return item;

        }
    }
}
