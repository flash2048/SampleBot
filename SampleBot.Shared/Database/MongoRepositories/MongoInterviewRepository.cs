using System.Collections.Generic;
using System.Configuration;
using MongoDB.Driver;
using SampleBot.Shared.Models;

namespace SampleBot.Shared.Database.MongoRepositories
{
    public class MongoInterviewRepository : IRepository<InterviewInfoModel>
    {
        private readonly IMongoDatabase _database;

        public MongoInterviewRepository()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString);
            _database = client.GetDatabase(ConfigurationManager.AppSettings["MongoDataBaseName"]);
        }

        public void Dispose()
        {
        }

        private IMongoCollection<InterviewInfoModel> Users => _database.GetCollection<InterviewInfoModel>("userinfo");

        public IEnumerable<InterviewInfoModel> GetAll()
        {
            return Users.Find(_ => true).ToList();
        }

        public InterviewInfoModel GetById(int id)
        {
            return Users.Find(x => x.Id == id).FirstOrDefault();
        }

        public void Add(InterviewInfoModel user)
        {
            int id = 0;
            var lastUser = Users.Find(_ => true).SortByDescending(x => x.Id).FirstOrDefault();
            if (lastUser != null)
            {
                id = lastUser.Id + 1;
            }
            user.Id = id;
            Users.InsertOne(user);
        }

        public void Update(InterviewInfoModel user)
        {
            var filter = Builders<InterviewInfoModel>.Filter.Eq(x => x.Id, user.Id);
            Users.ReplaceOne(filter, user);
        }

        public void Delete(int id)
        {
            var filter = Builders<InterviewInfoModel>.Filter.Eq(x => x.Id, id);
            Users.DeleteOne(filter);
        }

        public void Save()
        {
        }
    }
}
