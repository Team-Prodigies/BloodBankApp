using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BloodBankApp.CustomValidation
{
    public class NumbersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(validationContext.DisplayName + " cannot be empty!");
            var name = value.ToString();

            if (name.Any(char.IsDigit))
            {
                return new ValidationResult(validationContext.DisplayName + " cannot contain numbers!");
            }
            return ValidationResult.Success;
        }
    }
}
