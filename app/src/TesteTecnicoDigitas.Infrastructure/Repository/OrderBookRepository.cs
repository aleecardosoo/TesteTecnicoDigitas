using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using TesteTecnicoDigitas.Domain.Models;
using TesteTecnicoDigitas.Domain.Repository;

namespace TesteTecnicoDigitas.Infrastructure.Repository
{
    public class OrderBookRepository : IOrderBookRepository
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        private readonly IConfiguration _configuration;

        public OrderBookRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration["database:url"];
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(_configuration["database:order_book_database"]);
        }

        public async Task SaveOrderBookAsync(OrderBook orderBook, string instrument)
        {
            var collection = GetCollection(instrument);
            var document = orderBook.ToBsonDocument();

            await collection.InsertOneAsync(document);
        }

        public async Task<OrderBook> GetLastOrderBook(string instrument)
        {
            var collection = GetCollection(instrument);
            var filter = new BsonDocument();
            var sort = Builders<BsonDocument>.Sort.Descending("_id");
            var options = new FindOptions<BsonDocument>();
            options.Sort = sort;

            var result = await collection.FindAsync(filter, options);
            var lastDocument = result.FirstOrDefault();
            lastDocument.Remove("_id");
            var jsonDocument = lastDocument.ToJson();

            return JsonConvert.DeserializeObject<OrderBook>(jsonDocument);
        }

        private IMongoCollection<BsonDocument> GetCollection(string instrument)
        {
            instrument = instrument.ToLower().Replace("/", "");
            return _database.GetCollection<BsonDocument>($"{instrument}_collection");
        }
    }
}
