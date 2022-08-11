using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Data
{
    public class Seed
    {
        public static async Task SeedData(ApplicationDbContext context)
        {
            if (!context.Cities.Any())
            {
                var cities = new List<City>
                {
                    new City{
                        CityName="Gjilan"
                    },
                    new City{
                        CityName="Ferizaj"
                    },
                    new City{
                        CityName="Mitrovicë"
                    },
                     new City{
                        CityName="Prishtinë"
                    },
                     new City{
                        CityName="Suharekë"
                    },
                     new City{
                        CityName="Gjakovë"
                    },
                     new City{
                        CityName="Podujevë"
                    },
                     new City{
                        CityName="Pejë"
                    },
                     new City{
                        CityName="Prizren"
                    }
                };
                
                var bloodTypes = new List<BloodType>
            {
                new BloodType {
                    BloodTypeName = "A+"
                },
                new BloodType {
                    BloodTypeName = "A-"
                },
                new BloodType {
                    BloodTypeName = "B+"
                },
                new BloodType {
                    BloodTypeName = "B-"
                },
                 new BloodType {
                    BloodTypeName = "O+"
                },
                  new BloodType {
                    BloodTypeName = "O-"
                },
                  new BloodType {
                    BloodTypeName = "AB+"
                },
                  new BloodType {
                    BloodTypeName = "AB-"
                },
            };
                context.Cities.AddRange(cities);
                context.BloodTypes.AddRange(bloodTypes);
                await context.SaveChangesAsync();
            }
        }
    }
}
