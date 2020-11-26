using BetterBeastSaber.Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BetterBeastSaber.Data
{
    public class BetterBeastSaberContext : IBetterBeastSaberContext
    {
        private readonly IMongoDatabase _database = null;

        public BetterBeastSaberContext(IOptions<BetterBeastSaberDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Song> Songs => _database.GetCollection<Song>("Songs");
    }
}