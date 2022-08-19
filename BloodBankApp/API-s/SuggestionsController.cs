using BloodBankApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BloodBankApp.API_s {
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionsController : ControllerBase {
        private readonly ApplicationDbContext context;

        public SuggestionsController(ApplicationDbContext context) {
            this.context = context;
        }

        // GET: api/<SuggestionsController>
        [HttpGet]
        [Route("GetDonorsSuggestions")]
        public IEnumerable<string> GetDonorsSuggestions(string search) {

            if(search == null || search.Trim() == "") {
                return null;
            }
            var suggestions = context.Donors.Where(donor => donor.User.Name.ToUpper().Contains(search.ToUpper()) || donor.User.Surname.ToUpper().Contains(search.ToUpper())).Select(donor => donor.User.Name +" "+donor.User.Surname).Take(5).ToList<string>();

            return suggestions;
        }

        // GET api/<SuggestionsController>/5
        [HttpGet("{id}")]
        public string Get(int id) {
            return "value";
        }

        // POST api/<SuggestionsController>
        [HttpPost]
        public void Post([FromBody] string value) {
        }

        // PUT api/<SuggestionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/<SuggestionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
