using BloodBankApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.CustomValidation {
    public class HospitalCodeInUse : ValidationAttribute 
    {
        private readonly ApplicationDbContext _context;
        public HospitalCodeInUse(ApplicationDbContext context)
        {
            _context = context;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {

            var hospitalCode = value.ToString();
            var hospitalCodeInUse = _context.Hospitals
                .Where(hospital => hospital.HospitalCode == hospitalCode)
                .FirstOrDefault();

            if (hospitalCodeInUse != null)
            {
                return new ValidationResult("this hospital code is already taken!");
            }
            return ValidationResult.Success;
        }
    }
}
