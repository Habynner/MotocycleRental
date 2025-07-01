using challange_bikeRental.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using challange_bikeRental.Config;

namespace challange_bikeRental.Repositories.Bikes
{
    public class BikeRepository : IBikeRepository
    {
        private readonly IMongoCollection<Bike> _bikesCollection;

        public BikeRepository(IOptions<MongoDBSettings> settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _bikesCollection = database.GetCollection<Bike>("Motorcycles");
            CreateIndexes();
        }
        private void CreateIndexes()
        {
            var indexKeys = Builders<Bike>.IndexKeys.Ascending(b => b.Plate);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<Bike>(indexKeys, indexOptions);

            _bikesCollection.Indexes.CreateOne(indexModel);
        }

        public async Task<List<Bike>> GetAllAsync() => await _bikesCollection.Find(_ => true).ToListAsync();

        public async Task<Bike?> GetByIdAsync(string identificador) =>
            await _bikesCollection.Find(bike => bike.Id == identificador).FirstOrDefaultAsync();

        public async Task<Bike?> GetBikeByPlacaAsync(string placa)
        {
            return await _bikesCollection.Find(b => b.Plate == placa).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Bike bike)
        {
            await _bikesCollection.InsertOneAsync(bike);
        }

        public async Task UpdateBikeAsync(Bike bike)
        {
            var filter = Builders<Bike>.Filter.Eq(b => b.Id, bike.Id);
            var update = Builders<Bike>.Update.Set(b => b.Plate, bike.Plate);

            await _bikesCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string identificador) => await _bikesCollection.DeleteOneAsync(b => b.Id == identificador);
    }
}
