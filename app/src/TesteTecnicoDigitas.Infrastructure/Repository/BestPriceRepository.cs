using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using TesteTecnicoDigitas.Domain.Models;
using TesteTecnicoDigitas.Domain.Repository;

namespace TesteTecnicoDigitas.Infrastructure.Repository
{
    public class BestPriceRepository : IBestPriceRepository
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        private readonly IConfiguration _configuration;

        public BestPriceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration["database:url"];
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(_configuration["database:best_price_database"]);
        }

        public async Task SaveBestPriceAsync(BestPrice bestPrice, string instrument)
        {
            var collection = GetCollection(bestPrice.Operation, instrument);
            var document = bestPrice.ToBsonDocument();

            await collection.InsertOneAsync(document);
        }

        private IMongoCollection<BsonDocument> GetCollection(string operation, string instrument)
        {
            return _database.GetCollection<BsonDocument>($"{operation}_{instrument}_collection");
        }
    }
}
