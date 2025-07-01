using System.Text.Json.Serialization;

namespace challange_bikeRental.Models.DTOs
{
    public class UpdatePlacaDto
    {
        [JsonPropertyName("placa")]
        public string Plate { get; set; } = null!;
    }
}