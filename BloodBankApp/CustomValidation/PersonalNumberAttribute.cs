using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.CustomValidation
{
    public class PersonalNumberAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var personalNumber = value.ToString();

            if (personalNumber.Length != 10)
            {
                return new ValidationResult(validationContext.DisplayName + " should be 10 characters long!");
            }
            return ValidationResult.Success;
        }
    }
}
