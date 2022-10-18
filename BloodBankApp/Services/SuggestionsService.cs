using BloodBankApp.Data;
using BloodBankApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;

namespace BloodBankApp.Services
{
    public class SuggestionsService : ISuggestionsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsersService _usersService;

        public SuggestionsService(ApplicationDbContext context,
            IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public Task<IEnumerable<string>> GetDonorsSuggestionsAsync(string search)
        {
            if (search == null || search.Trim() == "")
            {
                return null;
            }

            var suggestions =  _context.Users
                .Where(user => user.Name.ToUpper()
                    .Contains(search.ToUpper()) || user.Surname.ToUpper()
                    .Contains(search.ToUpper()))
                .ToList()
                .Where(user => _usersService.UserIsInRole(user, "SuperAdmin").Result == false)
                .Select(user => user.Name + " " + user.Surname)
                .Take(5);

            return Task.FromResult(suggestions);
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