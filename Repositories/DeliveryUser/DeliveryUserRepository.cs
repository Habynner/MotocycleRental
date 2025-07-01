using challange_bikeRental.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using challange_bikeRental.Config;
using challange_bikeRental.Models.DTOs;

namespace challange_bikeRental.Repositories.DeliveryUser
{
    public class DeliveryUserRepository : IDeliveryUserRepository
    {
        private readonly IMongoCollection<User> _deliveryUsersCollection;

        public DeliveryUserRepository(IOptions<MongoDBSettings> settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _deliveryUsersCollection = database.GetCollection<User>("delivery_user");
            CreateIndexes();
        }
        private void CreateIndexes()
        {
            var cnpjIndexKeys = Builders<User>.IndexKeys.Ascending(u => u.Cnpj);
            var cnpjIndexOptions = new CreateIndexOptions { Unique = true };
            var cnpjIndexModel = new CreateIndexModel<User>(cnpjIndexKeys, cnpjIndexOptions);

            var cnhIndexKeys = Builders<User>.IndexKeys.Ascending(u => u.LicenseNumber);
            var cnhIndexOptions = new CreateIndexOptions { Unique = true };
            var cnhIndexModel = new CreateIndexModel<User>(cnhIndexKeys, cnhIndexOptions);

            _deliveryUsersCollection.Indexes.CreateMany(new[] { cnpjIndexModel, cnhIndexModel });
        }

        public async Task<List<User>> GetAllUsersAsync() => await _deliveryUsersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserByIdAsync(string identificador) =>
            await _deliveryUsersCollection.Find(user => user.Id == identificador).FirstOrDefaultAsync();

        public async Task<User?> GetUserByCnpjAsync(string cnpj)
        {
            return await _deliveryUsersCollection.Find(u => u.Cnpj == cnpj).FirstOrDefaultAsync();
        }
        public async Task<User?> GetUserByCnhAsync(string numeroCnh)
        {
            return await _deliveryUsersCollection.Find(u => u.LicenseNumber == numeroCnh).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _deliveryUsersCollection.InsertOneAsync(user);
        }

        public async Task UpdateUserCnhAsync(UpdateCnhDto updateDto)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, updateDto.Id);
            var update = Builders<User>.Update.Set(u => u.LicenseImage, updateDto.LicenseImage);

            var result = await _deliveryUsersCollection.UpdateOneAsync(filter, update);
            if (result.ModifiedCount == 0)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }
        }
    }
}
