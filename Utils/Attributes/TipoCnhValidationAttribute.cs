using System.ComponentModel.DataAnnotations;

namespace challange_bikeRental.Utils.Attributes
{
    public class TipoCnhValidationAttribute : ValidationAttribute
    {
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
