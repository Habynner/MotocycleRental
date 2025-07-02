using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using challange_bikeRental.Utils.Attributes;
using System.ComponentModel.DataAnnotations;

namespace challange_bikeRental.Models
{
    /// <summary>
    /// Represents a user in the motorcycle rental system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [JsonPropertyName("identificador")]
        [SwaggerSchema(ReadOnly = true)]
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [Required]
        [JsonPropertyName("nome")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Gets or sets the cnpj of the user.
        /// </summary>
        [Required]
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; } = null!;


        /// <summary>
        /// Gets or sets the birth date of the user.
        /// </summary>
        [Required]
        [JsonPropertyName("data_nascimento")]
        public DateTime BirthDate { get; set; }


        /// <summary>
        /// Gets or sets the license number of the user.
        /// </summary>
        [Required]
        [JsonPropertyName("numero_cnh")]
        public string LicenseNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the license type of the user.
        /// </summary>
        [Required]
        [TipoCnhValidation]
        [JsonPropertyName("tipo_cnh")]
        public string LicenseType { get; set; } = null!;


        /// <summary>
        /// Gets or sets the image of the user's license.
        /// </summary>
        [JsonPropertyName("imagem_cnh")]
        public string? LicenseImage { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the rented motorcycle.
        /// </summary>
        [JsonPropertyName("moto_alugada_id")]
        [SwaggerSchema(ReadOnly = true)]
        public string? RentedMotocycle { get; set; }
    }
}
