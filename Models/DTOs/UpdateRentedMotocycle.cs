using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace challange_bikeRental.Models.DTOs
{
    public class UpdateRentedMotocycleDto
    {
        [JsonPropertyName("identificador")]
        [SwaggerSchema(ReadOnly = true)]
        public string? Id { get; set; } = null!;

        [JsonPropertyName("data_devolucao")]
        public DateTime ReturnDate { get; set; }
    }
}