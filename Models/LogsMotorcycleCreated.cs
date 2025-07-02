using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace challange_bikeRental.Models
{
    /// <summary>
    /// Represents a rented motorcycle, including rental details and associated user and motorcycle.
    /// </summary>
    public class LogsMotorcycleCreated
    {
        /// <summary>
        /// Gets or sets the unique identifier for the rented motorcycle.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        [JsonPropertyName("identificador")]
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the identifier of the delivery user associated with the rented motorcycle.
        /// </summary>
        [JsonPropertyName("log")]
        public string Log { get; set; } = null!;
    }
}