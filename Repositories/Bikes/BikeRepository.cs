using challange_bikeRental.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using challange_bikeRental.Config.db;

namespace challange_bikeRental.Repositories.Bikes
{
    /// <summary>
    /// Repository for managing Bike entities in the MongoDB database.
    /// </summary>
    public class BikeRepository : IBikeRepository
    {
        private readonly IMongoCollection<Bike> _bikesCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BikeRepository"/> class.
        /// </summary>
        /// <param name="settings">The MongoDB settings.</param>
        /// <param name="mongoClient">The MongoDB client.</param>
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

        /// <summary>
        /// Retrieves all bikes from the collection asynchronously.
        /// </summary>
        /// <returns>A list of all <see cref="Bike"/> objects.</returns>
        public async Task<List<Bike>> GetAllAsync() => await _bikesCollection.Find(_ => true).ToListAsync();

        /// <summary>
        /// Retrieves a bike by its unique identifier asynchronously.
        /// </summary>
        /// <param name="identificador">The unique identifier of the bike.</param>
        /// <returns>The <see cref="Bike"/> object if found; otherwise, null.</returns>
        public async Task<Bike?> GetByIdAsync(string identificador) =>
            await _bikesCollection.Find(bike => bike.Id == identificador).FirstOrDefaultAsync();

        /// <summary>
        /// Retrieves a bike by its license plate asynchronously.
        /// </summary>
        /// <param name="placa">The license plate of the bike.</param>
        /// <returns>The <see cref="Bike"/> object if found; otherwise, null.</returns>
        public async Task<Bike?> GetBikeByPlacaAsync(string placa)
        {
            return await _bikesCollection.Find(b => b.Plate == placa).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts a new <see cref="Bike"/> into the collection asynchronously.
        /// </summary>
        /// <param name="bike">The <see cref="Bike"/> object to insert.</param>
        public async Task CreateAsync(Bike bike)
        {
            await _bikesCollection.InsertOneAsync(bike);
        }

        /// <summary>
        /// Updates the specified <see cref="Bike"/> in the collection asynchronously.
        /// </summary>
        /// <param name="bike">The <see cref="Bike"/> object to update.</param>
        public async Task UpdateBikeAsync(Bike bike)
        {
            var filter = Builders<Bike>.Filter.Eq(b => b.Id, bike.Id);
            var update = Builders<Bike>.Update.Set(b => b.Plate, bike.Plate);

            await _bikesCollection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Deletes a bike from the collection by its unique identifier asynchronously.
        /// </summary>
        /// <param name="identificador">The unique identifier of the bike to delete.</param>
        public async Task DeleteAsync(string identificador) => await _bikesCollection.DeleteOneAsync(b => b.Id == identificador);
    }
}
