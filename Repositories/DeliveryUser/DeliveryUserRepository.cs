using challange_bikeRental.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using challange_bikeRental.Config.db;
using challange_bikeRental.Models.DTOs;

namespace challange_bikeRental.Repositories.DeliveryUser
{
    /// <summary>
    /// Repository for managing delivery user data in the MongoDB database.
    /// </summary>
    public class DeliveryUserRepository : IDeliveryUserRepository
    {
        private readonly IMongoCollection<User> _deliveryUsersCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryUserRepository"/> class.
        /// </summary>
        /// <param name="settings">The MongoDB settings.</param>
        /// <param name="mongoClient">The MongoDB client.</param>
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

        /// <summary>
        /// Retrieves all delivery users from the database asynchronously.
        /// </summary>
        /// <returns>A list of all <see cref="User"/> objects.</returns>
        public async Task<List<User>> GetAllUsersAsync() => await _deliveryUsersCollection.Find(_ => true).ToListAsync();

        /// <summary>
        /// Retrieves a delivery user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="identificador">The unique identifier of the user.</param>
        /// <returns>The <see cref="User"/> object if found; otherwise, null.</returns>
        public async Task<User?> GetUserByIdAsync(string identificador) =>
            await _deliveryUsersCollection.Find(user => user.Id == identificador).FirstOrDefaultAsync();

        /// <summary>
        /// Retrieves a delivery user by their CNPJ asynchronously.
        /// </summary>
        /// <param name="cnpj">The CNPJ of the user.</param>
        /// <returns>The <see cref="User"/> object if found; otherwise, null.</returns>
        public async Task<User?> GetUserByCnpjAsync(string cnpj)
        {
            return await _deliveryUsersCollection.Find(u => u.Cnpj == cnpj).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Retrieves a delivery user by their CNH (driver's license number) asynchronously.
        /// </summary>
        /// <param name="numeroCnh">The CNH (driver's license number) of the user.</param>
        /// <returns>The <see cref="User"/> object if found; otherwise, null.</returns>
        public async Task<User?> GetUserByCnhAsync(string numeroCnh)
        {
            return await _deliveryUsersCollection.Find(u => u.LicenseNumber == numeroCnh).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new delivery user in the database asynchronously.
        /// </summary>
        /// <param name="user">The <see cref="User"/> object to create.</param>
        public async Task CreateUserAsync(User user)
        {
            await _deliveryUsersCollection.InsertOneAsync(user);
        }

        /// <summary>
        /// Updates the CNH (driver's license image) of a delivery user asynchronously.
        /// </summary>
        /// <param name="updateDto">The DTO containing the user ID and new license image.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
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
