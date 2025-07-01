using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using System.ComponentModel.DataAnnotations;

namespace challange_bikeRental.Models
{
    public class RentedBikes
    {
        [SwaggerSchema(ReadOnly = true)]
        [JsonPropertyName("identificador")]
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [SwaggerSchema(ReadOnly = true)]
        [JsonPropertyName("valor_diaria")]
        public int DailyRate { get; set; }

        [JsonPropertyName("entregador_id")]
        public string DeliveryUser { get; set; } = null!;

        [JsonPropertyName("moto_id")]
        public string MotocycleId { get; set; } = null!;

        [Required]
        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }

        [Required]
        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }

        [Required]
        [JsonPropertyName("data_previsao_termino")]
        public DateTime ForcastDate { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [JsonPropertyName("data_devolucao")]
        public DateTime? ReturnDate { get; set; } = null!;

        [Required]
        [JsonPropertyName("plano")]
        public int Plan { get; set; }
    }
}