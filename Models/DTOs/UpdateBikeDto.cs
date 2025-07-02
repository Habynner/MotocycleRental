using System.Text.Json.Serialization;

namespace challange_bikeRental.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for updating the plate information of a bike.
    /// </summary>
    public class UpdatePlacaDto
    {
        /// <summary>
        /// Gets or sets the plate information of the bike.
        /// </summary>
        [JsonPropertyName("placa")]
        public string Plate { get; set; } = null!;
    }
}