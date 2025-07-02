using challange_bikeRental.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using challange_bikeRental.Config.db;
using challange_bikeRental.Models.DTOs;

namespace challange_bikeRental.Repositories.RentedMotorcycles
{
    /// <summary>
    /// Repository for managing rented motorcycles.
    /// </summary>
    public class RentedMotorcycleRepository : IRentedMotorcycleRepository
    {
        private readonly IMongoCollection<RentedBikes> _rentedMotorcyclesCollection;
        private readonly IMongoCollection<User> _usersCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentedMotorcycleRepository"/> class.
        /// </summary>
        /// <param name="settings">The MongoDB settings.</param>
        /// <param name="mongoClient">The MongoDB client.</param>
        public RentedMotorcycleRepository(IOptions<MongoDBSettings> settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _rentedMotorcyclesCollection = database.GetCollection<RentedBikes>("rented_motorcycles");
            _usersCollection = database.GetCollection<User>("delivery_user");
        }

        /// <summary>
        /// Creates a new rental record in the rented motorcycles collection.
        /// </summary>
        /// <param name="rental">The rental information to insert.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateRentalAsync(RentedBikes rental)
        {
            await _rentedMotorcyclesCollection.InsertOneAsync(rental);
        }

        /// <summary>
        /// Updates the user with the specified ID to associate them with a rented motorcycle.
        /// </summary>
        /// <param name="entregadorId">The ID of the user (entregador) to update.</param>
        /// <param name="motoId">The ID of the rented motorcycle to associate with the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateUserWithRentedMotorcycleAsync(string entregadorId, string motoId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, entregadorId);
            var update = Builders<User>.Update.Set(u => u.RentedMotocycle, motoId);
            await _usersCollection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Retrieves a rented bike by its unique identifier.
        /// </summary>
        /// <param name="identificador">The unique identifier of the rented bike.</param>
        /// <returns>The rented bike if found; otherwise, null.</returns>
        public async Task<RentedBikes?> GetByIdAsync(string identificador) =>
            await _rentedMotorcyclesCollection.Find(bike => bike.Id == identificador).FirstOrDefaultAsync();

        /// <summary>
        /// Retrieves a rented bike by the motorcycle's unique identifier.
        /// </summary>
        /// <param name="motocycleId">The unique identifier of the motorcycle.</param>
        /// <returns>The rented bike if found; otherwise, null.</returns>
        public async Task<RentedBikes?> GetRentedByMotocycleAsync(string motocycleId)
        {
            return await _rentedMotorcyclesCollection.Find(b => b.MotocycleId == motocycleId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates the rented motorcycle record with the specified information.
        /// </summary>
        /// <param name="rental">The updated rental information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateRentedAsync(UpdateRentedMotocycleDto rental)
        {
            var filter = Builders<RentedBikes>.Filter.Eq(b => b.Id, rental.Id);
            var update = Builders<RentedBikes>.Update.Set(b => b.ReturnDate, rental.ReturnDate);

            await _rentedMotorcyclesCollection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Retrieves a rented bike by the motorcycle's license plate identifier.
        /// </summary>
        /// <param name="motocycleId">The unique identifier (license plate) of the motorcycle.</param>
        /// <returns>The rented bike if found; otherwise, null.</returns>
        public Task<RentedBikes?> GetBikeByPlacaAsync(string motocycleId)
        {
            throw new NotImplementedException();
        }
    }
}
