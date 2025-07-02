using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace challange_bikeRental.Models.DTOs
{
    /// <summary>
    /// DTO for updating CNH (driver's license) information.
    /// </summary>
    public class UpdateCnhDto
    {
        /// <summary>
        /// Gets or sets the identifier for the motorcycle rental.
        /// </summary>
        [JsonPropertyName("identificador")]
        [SwaggerSchema(ReadOnly = true)]
        public string? Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the image of the CNH (driver's license).
        /// </summary>
        [JsonPropertyName("imagem_cnh")]
        public string LicenseImage { get; set; } = null!;
    }
}