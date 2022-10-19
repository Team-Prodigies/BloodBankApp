using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.CustomValidation
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int _limit;
        public MinAgeAttribute(int limit)
        {
            _limit = limit;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(validationContext.DisplayName + " cannot be empty!");
            var bday = DateTime.Parse(value.ToString());
            var now = DateTime.Today;
            var age = now.Year - bday.Year;

            if (age >= 18 && (now.Month < bday.Month || now.Month == bday.Month && now.Day < bday.Day))
            {
                age--;
            }
            if (age < _limit)
            {
                return new ValidationResult("You must meet the minimum age required!");
            }
            return null;
        }
    }
}
