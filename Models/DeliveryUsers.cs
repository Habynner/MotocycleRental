using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using challange_bikeRental.Utils.Attributes;
using System.ComponentModel.DataAnnotations;

namespace challange_bikeRental.Models
{
    public class User
    {
        [JsonPropertyName("identificador")]
        [SwaggerSchema(ReadOnly = true)]
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [JsonPropertyName("nome")]
        public string Name { get; set; } = null!;

        [Required]
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; } = null!;

        [Required]
        [JsonPropertyName("data_nascimento")]
        public DateTime BirthDate { get; set; }

        [Required]
        [JsonPropertyName("numero_cnh")]
        public string LicenseNumber { get; set; } = null!;

        [Required]
        [TipoCnhValidation]
        [JsonPropertyName("tipo_cnh")]
        public string LicenseType { get; set; } = null!;


        [JsonPropertyName("imagem_cnh")]
        public string? LicenseImage { get; set; }

        [JsonPropertyName("moto_alugada_id")]
        [SwaggerSchema(ReadOnly = true)]
        public string? RentedMotocycle { get; set; }
    }
}
