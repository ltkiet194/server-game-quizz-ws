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
    internal class AccountHandler : IDbHandler<Account>
    {
        private readonly IGameDB<Account> _accountDB;
        public AccountHandler(IMongoDatabase database)
        {
            _accountDB = new MongoHandler<Account>(database);
        }
        public Account Create(Account item)
        {
            throw new NotImplementedException();
        }

        public Account Find(string id)
        {
            throw new NotImplementedException();
        }


        public Account FindByUserToken(string token)
        {
            var filter = Builders<Account>.Filter.Eq(i => i.Token.TokenId, token);
            return _accountDB.Get(filter);
        }
        public List<Account> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Account Update(string id, Account item)
        {
            throw new NotImplementedException();
        }
    }
}
