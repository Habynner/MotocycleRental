using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace challange_bikeRental.Models
{
    public class Bike
    {
        [JsonPropertyName("identificador")]
        [SwaggerSchema(ReadOnly = true)]
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("placa")]
        public string Plate { get; set; } = null!;

        [JsonPropertyName("modelo")]
        public string Model { get; set; } = null!;

        [JsonPropertyName("ano")]
        public decimal FabricationYear { get; set; }
    }
}