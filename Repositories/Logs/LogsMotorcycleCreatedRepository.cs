using challange_bikeRental.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using challange_bikeRental.Config.db;
using challange_bikeRental.Repositories.Logs;

namespace challange_bikeRental.Repositories.Logs
{
    /// <summary>
    /// Repository for managing rented motorcycles.
    /// </summary>
    public class LogMotorcycleCreatedRepository : ILogsMotorcycleCreatedRepository
    {
        private readonly IMongoCollection<LogsMotorcycleCreated> _logsMotorcyclesCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMotorcycleCreatedRepository"/> class.
        /// </summary>
        /// <param name="settings">The MongoDB settings.</param>
        /// <param name="mongoClient">The MongoDB client.</param>
        public LogMotorcycleCreatedRepository(IOptions<MongoDBSettings> settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _logsMotorcyclesCollection = database.GetCollection<LogsMotorcycleCreated>("logsMotocycleCreated");
            CreateIndexes();
        }
        /// <summary>
        /// Creates indexes for the logsMotorcycleCreated collection if needed.
        /// </summary>
        private void CreateIndexes()
        {
            var indexKeys = Builders<LogsMotorcycleCreated>.IndexKeys.Descending("CreatedAt");
            var indexModel = new CreateIndexModel<LogsMotorcycleCreated>(indexKeys);
            _logsMotorcyclesCollection.Indexes.CreateOne(indexModel);
        }
        /// <summary>
        /// Creates a new rental record in the log motorcycles collection.
        /// </summary>
        /// <param name="log">The rental information to insert.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateLogAsync(LogsMotorcycleCreated log)
        {
            await _logsMotorcyclesCollection.InsertOneAsync(log);
        }

    }
}
