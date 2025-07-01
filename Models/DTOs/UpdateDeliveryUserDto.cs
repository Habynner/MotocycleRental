using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace challange_bikeRental.Models.DTOs
{
    public class UpdateCnhDto
    {

        [JsonPropertyName("identificador")]
        [SwaggerSchema(ReadOnly = true)]
        public string? Id { get; set; } = null!;

        [JsonPropertyName("imagem_cnh")]
        public string LicenseImage { get; set; } = null!;
    }
    public class UpdateMotocycleDto
    {

        [JsonPropertyName("identificador")]
        [SwaggerSchema(ReadOnly = true)]
        public string? Id { get; set; } = null!;

        [JsonPropertyName("moto_alugada_id")]
        public string? RentedMotocycle { get; set; }
    }
}