using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using System.ComponentModel.DataAnnotations;

namespace challange_bikeRental.Models
{
    /// <summary>
    /// Represents a rented motorcycle, including rental details and associated user and motorcycle.
    /// </summary>
    public class RentedBikes
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
        /// Gets or sets the daily rental rate for the motorcycle.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        [JsonPropertyName("valor_diaria")]
        public int DailyRate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the delivery user associated with the rented motorcycle.
        /// </summary>
        [JsonPropertyName("entregador_id")]
        public string DeliveryUser { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the motorcycle associated with the rental.
        /// </summary>
        [JsonPropertyName("moto_id")]
        public string MotocycleId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the start date of the rental.
        /// </summary>
        [Required]
        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the rental.
        /// </summary>
        [Required]
        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the forecasted end date of the rental.
        /// </summary>
        [Required]
        [JsonPropertyName("data_previsao_termino")]
        public DateTime ForcastDate { get; set; }

        /// <summary>
        /// Gets or sets the return date of the rented motorcycle.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        [JsonPropertyName("data_devolucao")]
        public DateTime? ReturnDate { get; set; } = null!;

        /// <summary>
        /// Gets or sets the rental plan for the motorcycle.
        /// </summary>
        [Required]
        [JsonPropertyName("plano")]
        public int Plan { get; set; }
    }
}