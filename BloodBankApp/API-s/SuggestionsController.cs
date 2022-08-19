using BloodBankApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BloodBankApp.API_s {
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public SuggestionsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("GetDonorsSuggestions")]
        public async Task<IEnumerable<string>> GetDonorsSuggestionsAsync(string search)
        {

            if(search == null || search.Trim() == "") {
                return null;
            }
            var suggestions = await context.Donors.Where(donor => donor.User.Name.ToUpper().Contains(search.ToUpper()) || donor.User.Surname.ToUpper().Contains(search.ToUpper())).Select(donor => donor.User.Name +" "+donor.User.Surname).Take(5).ToListAsync<string>();

            return suggestions;
        }

        [HttpGet]
        [Route("GetHospitalsSuggestions")]
        public async Task<IEnumerable<string>> GetHospitalsSuggestionsAsync(string search)
        {

            if (search == null || search.Trim() == "") {
                return null;
            }
            var suggestions = await context.Hospitals.Where(hospital => hospital.HospitalName.ToUpper().Contains(search.ToUpper())).Select(hospital => hospital.HospitalName).Take(5).ToListAsync<string>();

            return suggestions;
        }
    }
}
