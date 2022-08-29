using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.CustomValidation
{
    public class PersonalNumberAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int personalNumber = Convert.ToInt32(value);

            if (personalNumber != 10)
            {
                return new ValidationResult(validationContext.DisplayName + " should be 10 characters long!");
            }
            return ValidationResult.Success;
        }
    }
}
