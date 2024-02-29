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
    public class UserHandler : IDbHandler<User>
    {
        private readonly IGameDB<User> _userDB;

        public UserHandler(IMongoDatabase database)
        {
            _userDB = new MongoHandler<User>(database);
        }

        public User Create(User item)
        {
            var user = _userDB.Create(item);
            return user;
        }

        public User Find(string id)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, id);
            return _userDB.Get(filter);
        }
        public List<User> FindAll()
        {
            return _userDB.GetAll();
        }
        public User FindByUserName(string username)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Username, username);
            return _userDB.Get(filter);
        }
        public User FindByDisplayName(string displayname)
        {
            var filter = Builders<User>.Filter.Eq(i => i.DisplayName, displayname);
            return _userDB.Get(filter);
        }
        public void Remove(string id)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, id);
            _userDB.Remove(filter);
        }

        public User Update(User item)
        {
            throw new NotImplementedException();
        }

        public User Update(string id, User item)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, id);
            var updater = Builders<User>.Update
                .Set(i => i.Password, item.Password)
                .Set(i => i.DisplayName, item.DisplayName)
                .Set(i => i.Amount, item.Amount)
                .Set(i => i.Score, item.Score)
                .Set(i => i.Avatar, item.Avatar)
                .Set(i => i.UpdatedAt, DateTime.Now)
                .Set(i => i.IsOnline, item.IsOnline);
            _userDB.Update(filter, updater);
            return item;
        }
    }
}
