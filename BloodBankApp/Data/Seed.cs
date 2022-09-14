using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Controllers;
using System.Security.Claims;
using BloodBankApp.Areas.SuperAdmin.Permission;

namespace BloodBankApp.Data
{
    public static class Seed
    {
        public static async Task SeedData(ApplicationDbContext context, RoleManager<IdentityRole<Guid>> roleManager)
        {
            if (!context.Cities.Any() && !context.Hospitals.Any() && !context.Locations.Any())
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
                    },
                     new City{
                         CityName="Drenas"
                     },
                     new City{
                         CityName="Malishevë"
                     },
                     new City{
                         CityName="Viti"
                     }, new City{
                         CityName="Rahovec"
                     },
                     new City{
                         CityName="Vushtrri"
                     }
                };
                context.Cities.AddRange(cities);

                var locations = new List<Location>
                {
                    new Location
                    {
                        Longitude = "42.457937",
                        Latitude = "21.462887"
                    },
                    new Location
                    {
                        Longitude = "42.457953",
                        Latitude = "21.462877"
                    },
                    new Location
                    {
                        Longitude = "42.643420",
                        Latitude = "21.160862"
                    },
                    new Location
                    {
                        Longitude = "42.662377",
                        Latitude = "20.273710"
                    },
                    new Location
                    {
                        Longitude = "42.203777",
                        Latitude = "20.730347"
                    }
                };
                context.Locations.AddRange(locations);

                var hospitals = new List<Hospital>
                {
                    new Hospital
                    {
                        HospitalName = "Spitali Rajonal Idriz Seferi",
                        ContactNumber = "+38344111222",
                        HospitalCode = "203452",
                        Location = locations[0],
                        City = cities[0],
                    },
                    new Hospital
                    {
                        HospitalName = "Spitali Rajonal I Ferizajit",
                        ContactNumber = "+38344342354",
                        HospitalCode = "304923",
                        Location = locations[1],
                        City = cities[1],
                    },
                    new Hospital
                    {
                        HospitalName = "Spitali I Përgjithshëm Dr. Sami Haxhibeqiri",
                        ContactNumber = "+38344997799",
                        HospitalCode = "345672",
                        Location = locations[2],
                        City = cities[2],
                    },
                    new Hospital
                    {
                        HospitalName = "Qendra Klinike Universitare e Kosovës",
                        ContactNumber = "+38344334554",
                        HospitalCode = "256552",
                        Location = locations[3],
                        City = cities[3],
                    },
                    new Hospital
                    {
                        HospitalName = "Spitali I Përgjithshëm Shkronjat",
                        ContactNumber = "+38345995445",
                        HospitalCode = "229564",
                        Location = locations[4],
                        City = cities[4],
                    },
                };
                context.Hospitals.AddRange(hospitals);
            }
            if (!context.Roles.Any())
            {
                var roles = new List<IdentityRole<Guid>>
                {
                    new IdentityRole<Guid>
                    {
                        Name = "Donor"
                    },
                    new IdentityRole<Guid>
                    {
                        Name = "HospitalAdmin"
                    },
                    new IdentityRole<Guid>
                    {
                        Name = "SuperAdmin"
                    }
                };
                context.Roles.AddRange(roles);
            }
            if (!context.BloodTypes.Any())
            {
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
                    }
                };
                context.BloodTypes.AddRange(bloodTypes);
            }

            if (!context.RoleClaims.Any())
            {
                await SeedClaimsForSuperAdmin(roleManager);
                await SeedClaimsForDonor(roleManager);
                await SeedClaimsForHospitalAdmin(roleManager);
            }
            await context.SaveChangesAsync();
        }
        public async static Task SeedClaimsForSuperAdmin(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            await roleManager.AddPermissionClaim(adminRole, "Cities");
            await roleManager.AddPermissionClaim(adminRole, "Hospitals");
            await roleManager.AddPermissionClaim(adminRole, "Roles");
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.Roles.ViewPermissions));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.Roles.EditPermissions));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.Roles.AddPermissions));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.SuperAdmin.Create));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.SuperAdmin.ViewStatistics));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.SuperAdmin.EditProfile));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.SuperAdmin.ViewProfile));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.SuperAdmin.ChangePassword));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.Donors.View));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.Donors.Lock));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.Donors.Unlock));
        }
        public async static Task SeedClaimsForDonor(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var donorRole = await roleManager.FindByNameAsync("Donor");
            await roleManager.AddClaimAsync(donorRole, new Claim("Permission", Permissions.Donors.ViewProfile));
            await roleManager.AddClaimAsync(donorRole, new Claim("Permission", Permissions.Donors.EditProfile));
            await roleManager.AddClaimAsync(donorRole, new Claim("Permission", Permissions.Donors.DeleteAccount));
            await roleManager.AddClaimAsync(donorRole, new Claim("Permission", Permissions.Donors.ChangePassword));
            await roleManager.AddClaimAsync(donorRole, new Claim("Permission", Permissions.Donors.ViewDashboard));
        }
        public async static Task SeedClaimsForHospitalAdmin(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var hospitalAdminRole = await roleManager.FindByNameAsync("HospitalAdmin");
            await roleManager.AddClaimAsync(hospitalAdminRole, new Claim("Permission", Permissions.HospitalAdmin.ViewDashboard));
            await roleManager.AddClaimAsync(hospitalAdminRole, new Claim("Permission", Permissions.HospitalAdmin.EditHospital));
            await roleManager.AddClaimAsync(hospitalAdminRole, new Claim("Permission", Permissions.HospitalAdmin.ViewHospital));
            await roleManager.AddClaimAsync(hospitalAdminRole, new Claim("Permission", Permissions.HospitalAdmin.EditProfile));
            await roleManager.AddClaimAsync(hospitalAdminRole, new Claim("Permission", Permissions.HospitalAdmin.ViewProfile));
            await roleManager.AddClaimAsync(hospitalAdminRole, new Claim("Permission", Permissions.HospitalAdmin.ChangePassword));
        }
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole<Guid>> roleManager, IdentityRole<Guid> role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}