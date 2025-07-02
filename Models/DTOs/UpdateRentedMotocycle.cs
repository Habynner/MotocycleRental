using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace challange_bikeRental.Models.DTOs
{
    /// <summary>
    /// DTO for updating a rented motorcycle, including its identifier and return date.
    /// </summary>
    public class UpdateRentedMotocycleDto
    {
        /// <summary>
        /// Gets or sets the identifier of the rented motorcycle.
        /// </summary>
        [JsonPropertyName("identificador")]
        [SwaggerSchema(ReadOnly = true)]
        public string? Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the return date of the rented motorcycle.
        /// </summary>
        [JsonPropertyName("data_devolucao")]
        public DateTime ReturnDate { get; set; }
    }
}