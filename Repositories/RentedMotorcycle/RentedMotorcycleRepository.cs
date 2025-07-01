using challange_bikeRental.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using challange_bikeRental.Config;
using challange_bikeRental.Models.DTOs;

namespace challange_bikeRental.Repositories.RentedMotorcycles
{
    public class RentedMotorcycleRepository : IRentedMotorcycleRepository
    {
        private readonly IMongoCollection<RentedBikes> _rentedMotorcyclesCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public RentedMotorcycleRepository(IOptions<MongoDBSettings> settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _rentedMotorcyclesCollection = database.GetCollection<RentedBikes>("rented_motorcycles");
            _usersCollection = database.GetCollection<User>("delivery_user");
        }

        public async Task CreateRentalAsync(RentedBikes rental)
        {
            await _rentedMotorcyclesCollection.InsertOneAsync(rental);
        }

        public async Task UpdateUserWithRentedMotorcycleAsync(string entregadorId, string motoId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, entregadorId);
            var update = Builders<User>.Update.Set(u => u.RentedMotocycle, motoId);
            await _usersCollection.UpdateOneAsync(filter, update);
        }

        public async Task<RentedBikes?> GetByIdAsync(string identificador) =>
            await _rentedMotorcyclesCollection.Find(bike => bike.Id == identificador).FirstOrDefaultAsync();

        public async Task<RentedBikes?> GetRentedByMotocycleAsync(string motocycleId)
        {
            return await _rentedMotorcyclesCollection.Find(b => b.MotocycleId == motocycleId).FirstOrDefaultAsync();
        }

        public async Task UpdateRentedAsync(UpdateRentedMotocycleDto rental)
        {
            var filter = Builders<RentedBikes>.Filter.Eq(b => b.Id, rental.Id);
            var update = Builders<RentedBikes>.Update.Set(b => b.ReturnDate, rental.ReturnDate);

            await _rentedMotorcyclesCollection.UpdateOneAsync(filter, update);
        }

        public Task<RentedBikes?> GetBikeByPlacaAsync(string motocycleId)
        {
            throw new NotImplementedException();
        }
    }
}
