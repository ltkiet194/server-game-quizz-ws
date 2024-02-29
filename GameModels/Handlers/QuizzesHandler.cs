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
    public class QuizzesHandler : IDbHandler<Quizzes>
    {
        private readonly IGameDB<Quizzes> _quizzesDB;
        public QuizzesHandler(IMongoDatabase database)
        {
            _quizzesDB = new MongoHandler<Quizzes>(database);
        }
        public Quizzes Create(Quizzes item)
        {
            throw new NotImplementedException();
        }

        public Quizzes Find(string id)
        {
            var filter = Builders<Quizzes>.Filter.Eq(i => i.Id, id);
            return _quizzesDB.Get(filter);
        }

        public List<Quizzes> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Quizzes Update(string id, Quizzes item)
        {
            throw new NotImplementedException();
        }
    }
}
