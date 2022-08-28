using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.CustomValidation
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private int _Limit;
        public MinAgeAttribute(int Limit)
        {
            _Limit = Limit;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime bday = DateTime.Parse(value.ToString());
            DateTime now = DateTime.Today;
            int age = now.Year - bday.Year;

            if (age >= 18 && (now.Month < bday.Month || now.Month == bday.Month && now.Day < bday.Day))
            {
                age--;
            }
            if (age < _Limit)
            {
                return new ValidationResult("You must meet the minimum age required!");
            }
            return null;
        }
    }
}
