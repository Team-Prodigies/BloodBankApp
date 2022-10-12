using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICitiesService _citiesService;
        private readonly IHospitalService _hospitalService;
        private readonly IBloodTypesService _bloodTypesService;

        public NotificationService(ApplicationDbContext context,
            ICitiesService citiesService,
            IHospitalService hospitalService,
            IBloodTypesService bloodTypesService)
        {
            _context = context;
            _citiesService = citiesService;
            _hospitalService = hospitalService;
            _bloodTypesService = bloodTypesService;
        }

        public async Task<bool> SendNotificationToDonors(DonationPost post)
        {
            var hospital = await _hospitalService.GetHospital(post.HospitalId);
            var city = await _citiesService.GetCity(hospital.CityId);
            var bloodType = await _bloodTypesService.GetBloodType(post.BloodTypeId);

            var potentialDonors = await GetPotentialDonors(bloodType.BloodTypeName, city.CityId);

            if (potentialDonors.Count <= 0) return false;
            {
                var notification = new Notification
                {
                    Description = $"In the city of {city.CityName} on hospital {hospital.HospitalName} ,{post.AmountRequested} ml of {bloodType.BloodTypeName}"
                                                    + " blood type are needed.",
                    DonationPostId = post.DonationPostId
                };
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                foreach (var userNotification in potentialDonors.Select(donor => new UserNotifications
                         {
                             Id = donor.DonorId,
                             NotificationId = notification.NotificationId
                         }))
                {
                    _context.UserNotifications.Add(userNotification);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
        }

        public async Task<bool> SendNotificationToDonors(BloodReserveModel reserve, Guid hospitalId)
        {
            var potentialDonors = await GetPotentialDonors(reserve, hospitalId);
            if (!potentialDonors.Any()) 
                return false;

            var hospital = await _context.Hospitals.FindAsync(hospitalId);
            var city = await _context.Cities.FindAsync(hospital.CityId);
            var bloodType = await _context.BloodTypes.FindAsync(reserve.BloodTypeId);
            var notification = new Notification
            {
                Description = $"Blood of type {bloodType.BloodTypeName} is needed on {hospital.HospitalName}, {city.CityName} "
            };
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            foreach (var userNotification in potentialDonors.Select(donor => new UserNotifications
            {
                Id = donor.DonorId,
                NotificationId = notification.NotificationId
            }))
            {
                _context.UserNotifications.Add(userNotification);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        private async Task<List<Donor>> GetPotentialDonors(BloodReserveModel reserve, Guid hospitalId)
        {
            var cityId = await _context.Hospitals.Where(hospital => hospital.HospitalId == hospitalId).Select(hospital => hospital.CityId).FirstOrDefaultAsync();

            var potentialDonors = await _context.Donors
                .Include(donor => donor.BloodDonations)
                .Where(donor => donor.BloodTypeId == reserve.BloodTypeId
                                && donor.CityId == cityId)
                .ToListAsync();

            potentialDonors = potentialDonors.Where(donor => !donor.BloodDonations.Any() || donor.Gender == Gender.FEMALE
                    ? donor.BloodDonations.All(donation => (DateTime.Now - donation.DonationDate).Days > 120)
                    : donor.BloodDonations.All(donation => (DateTime.Now - donation.DonationDate).Days > 90))
                .ToList();

            return potentialDonors;
        }

        public async Task<List<Notification>> GetNotificationsForUser(string userId)
        {
            var notifications = await _context.UserNotifications
                .Include(x=>x.Notification)
                .Where(x => x.Id == Guid.Parse(userId))
                .Select(x=>x.Notification)
                .Take(4)
                .ToListAsync();

            return notifications;
        }

        private async Task<List<Donor>> GetPotentialDonors(string bloodTypeName, Guid cityId)
        {
            var potentialDonors = new List<Donor>();
            switch (bloodTypeName)
            {
                case "A+":
                {
                    var donors = await _context.Donors
                        .Where(x => (x.BloodType.BloodTypeName.Equals("A+")
                                     || x.BloodType.BloodTypeName.Equals("AB+"))
                                    && x.CityId == cityId)
                        .ToListAsync();
                    if (!donors.Any()) break;
                    await CheckLastDonation(donors, potentialDonors);
                    break;
                }
                case "A-":
                {
                    var donors = await _context.Donors
                        .Where(x => (x.BloodType.BloodTypeName.Equals("A+")
                                     || x.BloodType.BloodTypeName.Equals("A-")
                                     || x.BloodType.BloodTypeName.Equals("AB+")
                                     || x.BloodType.BloodTypeName.Equals("AB-"))
                                    && x.CityId == cityId)
                        .ToListAsync();
                    if (!donors.Any()) break;
                    await CheckLastDonation(donors, potentialDonors);
                    break;
                }
                case "B+":
                {
                    var donors = await _context.Donors
                        .Where(x => (x.BloodType.BloodTypeName.Equals("B+")
                                     || x.BloodType.BloodTypeName.Equals("AB+"))
                                    && x.CityId == cityId)
                        .ToListAsync();
                    if (!donors.Any()) break;
                    await CheckLastDonation(donors, potentialDonors);
                    break;
                }
                case "B-":
                {
                    var donors = await _context.Donors
                        .Where(x => (x.BloodType.BloodTypeName.Equals("B+")
                                     || x.BloodType.BloodTypeName.Equals("AB+")
                                     || x.BloodType.BloodTypeName.Equals("B-")
                                     || x.BloodType.BloodTypeName.Equals("AB-"))
                                    && x.CityId == cityId)
                        .ToListAsync();
                    if (!donors.Any()) break;
                    await CheckLastDonation(donors, potentialDonors);
                    break;
                }
                case "O+":
                {
                    var donors = await _context.Donors
                        .Where(x => (x.BloodType.BloodTypeName.Equals("O+")
                                     || x.BloodType.BloodTypeName.Equals("A+")
                                     || x.BloodType.BloodTypeName.Equals("B+")
                                     || x.BloodType.BloodTypeName.Equals("AB+"))
                                    && x.CityId == cityId)
                        .ToListAsync();
                    if (!donors.Any()) break;
                    await CheckLastDonation(donors, potentialDonors);
                    break;
                }
                case "O-":
                {
                    var donors = await _context.Donors
                        .Where(x => x.CityId == cityId)
                        .ToListAsync();
                    if (!donors.Any()) break;
                    await CheckLastDonation(donors, potentialDonors);
                    break;
                }
                case "AB+":
                {
                    var donors = await _context.Donors
                        .Where(x => x.BloodType.BloodTypeName.Equals("AB+")
                                    && x.CityId == cityId)
                        .ToListAsync();
                    if (!donors.Any()) break;
                    await CheckLastDonation(donors, potentialDonors);
                    break;
                }
                case "AB-":
                {
                    var donors = await _context.Donors
                        .Where(x => (x.BloodType.BloodTypeName.Equals("AB+")
                                     || x.BloodType.BloodTypeName.Equals("AB-"))
                                    && x.CityId == cityId)
                        .ToListAsync();
                    if (!donors.Any()) break;
                    await CheckLastDonation(donors, potentialDonors);
                    break;
                }
            }

            return potentialDonors;
        }

        private async Task CheckLastDonation(List<Donor> donors, List<Donor> potentialDonors)
        {
            foreach (var donor in donors)
            {
                var donation = await _context.BloodDonations
                    .OrderByDescending(x => x.DonationDate)
                    .FirstOrDefaultAsync(d => d.DonorId == donor.DonorId);
                if (donation == null)
                {
                    potentialDonors.Add(donor);
                }
                else
                    switch (donor.Gender)
                    {
                        case Gender.MALE when donation.DonationDate <= DateTime.Now.AddMonths(-3):
                        case Gender.FEMALE when donation.DonationDate <= DateTime.Now.AddMonths(-4):
                            potentialDonors.Add(donor);
                            break;
                    }
            }
        }
    }
}