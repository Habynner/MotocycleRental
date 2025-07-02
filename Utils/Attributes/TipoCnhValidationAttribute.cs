using System.ComponentModel.DataAnnotations;

namespace challange_bikeRental.Utils.Attributes
{
    /// <summary>
    /// Validation attribute to ensure the CNH type is 'A', 'B', or 'A+B'.
    /// </summary>
    public class TipoCnhValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value of CNH type is valid ('A', 'B', or 'A+B').
        /// </summary>
        /// <param name="value">The value of the CNH type to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether validation succeeded.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var tipoCnh = value as string;

            if (tipoCnh is not ("A" or "B" or "A+B"))
            {
                return new ValidationResult("O tipo da CNH deve ser 'A', 'B' ou 'A+B'.");
            }

            return ValidationResult.Success;
        }
    }
}
