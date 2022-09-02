using BloodBankApp.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BloodBankApp.CustomValidation {
    public class HospitalCodeInUse : ValidationAttribute 
    {
        private readonly ApplicationDbContext _context;
        public HospitalCodeInUse(ApplicationDbContext context)
        {
            _context = context;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value == null)
            {
                return new ValidationResult(validationContext.DisplayName + " cannot be empty!");
            }

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
