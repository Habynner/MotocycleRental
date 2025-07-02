using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace challange_bikeRental.Models
{
    /// <summary>
    /// Represents a bike available for rental.
    /// </summary>
    public class Bike
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bike.
        /// </summary>
        [JsonPropertyName("identificador")]
        [SwaggerSchema(ReadOnly = true)]
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the plate number of the bike.
        /// </summary>
        [JsonPropertyName("placa")]
        public string Plate { get; set; } = null!;

        /// <summary>
        /// Gets or sets the model of the bike.
        /// </summary>
        [JsonPropertyName("modelo")]
        public string Model { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fabrication year of the bike.
        /// </summary>
        [JsonPropertyName("ano")]
        public decimal FabricationYear { get; set; }
    }
}