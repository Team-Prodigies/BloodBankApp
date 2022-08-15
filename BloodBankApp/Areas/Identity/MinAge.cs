using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
            if (now < bday.AddYears(-age))
            {
                age--;
            }
            if (age < _Limit)
            {
                var result = new ValidationResult("Sorry you are not old enough");
                return result;
            }


            return null;

        }
    }
}
