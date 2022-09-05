using BloodBankApp.Data;
using BloodBankApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Services
{
    public class SuggestionsService : ISuggestionsService
    {
        private readonly ApplicationDbContext _context;

        public SuggestionsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetDonorsSuggestionsAsync(string search)
        {
            if (search == null || search.Trim() == "")
            {
                return null;
            }
            var suggestions = await _context.Donors
                .Where(donor => donor.User.Name.ToUpper()
                .Contains(search.ToUpper()) || donor.User.Surname.ToUpper()
                .Contains(search.ToUpper()))
                .Select(donor => donor.User.Name + " " + donor.User.Surname)
                .Take(5).ToListAsync();

            return suggestions;
        }

        public async Task<IEnumerable<string>> GetHospitalsSuggestionsAsync(string search)
        {
            if (search == null || search.Trim() == "")
            {
                return null;
            }
            var suggestions = await _context.Hospitals
                .Where(hospital => hospital.HospitalName.ToUpper()
                .Contains(search.ToUpper()))
                .Select(hospital => hospital.HospitalName)
                .Take(5).ToListAsync();

            return suggestions;
        }
    }
}