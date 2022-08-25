using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.Identity.Pages.Account
{
    public class MinAge : ValidationAttribute
    {
        private int _Limit;
        public MinAge(int Limit)
        { 
            _Limit = Limit;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime bday = DateTime.Parse(value.ToString());
            DateTime now = DateTime.Today;
            int age = now.Year - bday.Year;
            
            if ((age >= 18) && ((now.Month < bday.Month) || (now.Month == bday.Month && now.Day < bday.Day)))
            {
                age--;
            }
            if (age < _Limit)
            {
                var result = new ValidationResult("You must meet the minimum age required!");
                return result;
            }
            return null;

        }
    }
}
