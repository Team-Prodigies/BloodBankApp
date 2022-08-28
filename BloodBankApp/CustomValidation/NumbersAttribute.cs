using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BloodBankApp.CustomValidation
{
    public class NumbersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value.ToString();

            if (name.Any(x => char.IsDigit(x)))
            {
                return new ValidationResult(validationContext.DisplayName + " cannot contain numbers!");
            }
            return ValidationResult.Success;
        }
    }
}
