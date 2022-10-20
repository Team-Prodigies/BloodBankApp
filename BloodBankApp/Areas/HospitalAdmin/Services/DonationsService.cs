using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.HospitalAdmin.Model;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class DonationsService : IDonationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsersService _usersService;
        private readonly IEmail _mailService;
        private readonly INotyfService _notyfService;
        private readonly IDonorsService _donorsService;

        public DonationsService(ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IUsersService usersService, 
            IEmail mailService,
            INotyfService notyfService,
            IDonorsService donorsService)
        {
            _context = context;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _usersService = usersService;
            _mailService = mailService;
            _notyfService = notyfService;
            _donorsService = donorsService;
        }

        public async Task<Guid> GetCurrentHospitalId()
        {
            var user = await _usersService.GetUser(_contextAccessor.HttpContext.User);
            var hospitalAdmin = await _context.MedicalStaffs.Where(staff => staff.MedicalStaffId == user.Id).FirstOrDefaultAsync();
            return await _context.Hospitals.Where(hospital => hospital.HospitalId == hospitalAdmin.HospitalId).Select(hospital => hospital.HospitalId).FirstOrDefaultAsync();
        }

        public async Task<bool> AddDonationRequest(Guid postId, Guid donorId)
        {
            var getDonor = await _donorsService.GetDonor(donorId);

            if (getDonor == null) return false;
            
            var request = new DonationRequests
            {
                DonationPostId = postId,
                DonorId = getDonor.DonorId,
                RequestDate = DateTime.Now
            };
            await _context.DonationRequest.AddAsync(request);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<BloodDonationModel>> GetAllDonations(string? searchTerm)
        {
            var hospitalId = await GetCurrentHospitalId();
            var donations = await _context.BloodDonations
                .Include(donation => donation.Hospital)
                .Include(donation => donation.Donor)
                    .ThenInclude(donor => donor.User)
                .Where(donation => donation.HospitalId == hospitalId && (string.IsNullOrEmpty(searchTerm) || (donation.Donor.User.Name + donation.Donor.User.Surname).Replace(" ", "").ToUpper().Contains(searchTerm.Replace(" ", "").ToUpper())))
                .ToListAsync();
            var result = _mapper.Map<List<BloodDonationModel>>(donations);
            return result;
        }

        public async Task<List<DonationRequests>> GetAllDonationRequests()
        {
            var hospitalId = await GetCurrentHospitalId();
            return await _context.DonationRequest
                .Include(request => request.Donor)
                    .ThenInclude(donor => donor.User)
                .Include(request => request.DonationPost)
                    .ThenInclude(donor => donor.Hospital)
                .Where(request => request.DonationPost.HospitalId == hospitalId)
                .ToListAsync();
        }

        public async Task<bool> AddDonation(BloodDonationModel donation)
        {
            try
            {
                var hospitalId = await GetCurrentHospitalId();
                var bloodDonation = _mapper.Map<BloodDonation>(donation);
                bloodDonation.HospitalId = hospitalId;
                bloodDonation.DonorId = donation.Donor.DonorId;
                bloodDonation.Donor = null;
                bloodDonation.DonationPostId = null;
                await _context.BloodDonations.AddAsync(bloodDonation);
                await _context.SaveChangesAsync();
                var numberOfDonations = await GetNumberOfDonations(donation.Donor.DonorId) % 3;
                if (numberOfDonations == 0)
                {
                   var result = await SendEMailToDonor(donation.Donor.DonorId);
                   if (result)
                   {
                       _notyfService.Success("Email was sent to the donor for free check up");
                   }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> SendEMailToDonor(Guid donorId)
        {
            var user = await _context.Users.FindAsync(donorId);
            var emailData = new EmailData
            {
                EmailToId = user.Email,
                EmailToName = user.Name + user.Surname,
                EmailSubject = "Free Check-Up",
                EmailBody = "Congratulations!" +
                            "\nYou have earned a free check up!"
            };
            return await _mailService.SendEmail(emailData);
        }

        private async Task<int> GetNumberOfDonations(Guid donorId)
        {
            return await _context.BloodDonations.Where(donation => donation.DonorId == donorId).CountAsync();
        }

        public async Task<bool> UpdateDonation(BloodDonationModel donation)
        {
            var dbDonation = await _context.BloodDonations.FindAsync(donation.BloodDonationId);
            dbDonation.Amount = donation.Amount;
            dbDonation.DonationDate = donation.DonationDate;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<BloodDonationModel> GetDonation(Guid donationId)
        {
            var donation = await _context.BloodDonations
                .Include(donation => donation.Donor)
                .ThenInclude(donor => donor.BloodType)
                .Include(donation => donation.Donor.User)
                .FirstOrDefaultAsync(donation => donation.BloodDonationId == donationId);

            return _mapper.Map<BloodDonationModel>(donation);
        }

        public async Task<bool> ApproveDonationRequest(Guid requestId, double amount)
        {
            var request = await _context.DonationRequest.FindAsync(requestId);
            var donation = _mapper.Map<BloodDonation>(request);
            var hospitalId = await GetCurrentHospitalId();
            donation.HospitalId = hospitalId;
            donation.Amount = amount;
            donation.DonationDate = DateTime.Now;

            await _context.BloodDonations.AddAsync(donation);
            await _context.SaveChangesAsync();

            var result = await RemoveDonationRequest(request.Id);

            if (result)
            {
                var numberOfDonations = await GetNumberOfDonations(request.DonorId) % 3;
                if (numberOfDonations == 0)
                {
                    var emailResult = await SendEMailToDonor(request.DonorId);
                    if (emailResult)
                    {
                        _notyfService.Success("Email was sent to the donor for free check up");
                    }
                    return emailResult;
                }
            }

            return result;
        }

        public async Task<bool> RemoveDonationRequest(Guid requestId)
        {
            var request = await _context.DonationRequest.FindAsync(requestId);
            _context.DonationRequest.Remove(request);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}