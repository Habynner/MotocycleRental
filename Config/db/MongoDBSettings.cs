namespace challange_bikeRental.Config.db
{
    /// <summary>
    /// Represents the settings required to connect to a MongoDB database.
    /// </summary>
    public class MongoDBSettings
    {
        /// <summary>
        /// Gets or sets the connection string used to connect to the MongoDB database.
        /// </summary>
        /// <summary>
        /// Gets or sets the name of the MongoDB database.
        /// </summary>
        public string DatabaseName { get; set; } = null!;
        /// <summary>
        /// Gets or sets the connection string used to connect to the MongoDB database.
        /// </summary>
        public string ConnectionString { get; set; } = null!;
    }
}