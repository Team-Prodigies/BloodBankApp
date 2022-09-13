using AutoMapper;
using BloodBankApp.Areas.Identity.Pages.Account;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BloodBankApp.Areas.Identity.Services.Interfaces;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.ExtensionMethods;
using static BloodBankApp.Areas.Identity.Pages.Account.RegisterMedicalStaffModel;

namespace BloodBankApp.Areas.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMedicalStaffService _medicalStaffService;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IDonorsService _donorsService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersService(
            IMedicalStaffService medicalStaffService,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IDonorsService donorsService,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _medicalStaffService = medicalStaffService;
            _signInManager = signInManager;
            _context = context;
            _donorsService = donorsService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> AddSuperAdmin(SuperAdminModel user)
        {
            user.Name = user.Name.ToTitleCase();
            user.Surname = user.Surname.ToTitleCase();
            var superAdminAccount = _mapper.Map<User>(user);

            var result = await _userManager.CreateAsync(superAdminAccount, user.Password);

            if (result.Succeeded)
            {
                return await _userManager.AddToRoleAsync(superAdminAccount, "SuperAdmin");
            }
            return result;
        }

        public async Task<IdentityResult> EditSuperAdmin(User user)
        {
            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<IdentityResult> ChangePassword(User user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            return result;
        }

        public async Task<User> GetUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            return _userManager.GetUserId(principal);
        }

        public async Task<IdentityResult> SetPhoneNumber(User user, string phoneNumber)
        {
            return await _userManager.SetPhoneNumberAsync(user, phoneNumber);
        }

        public async Task<IdentityResult> SetUserName(User user, string username)
        {
            return await _userManager.SetUserNameAsync(user, username);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _userManager.Users
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();
        }

        public async Task<IdentityResult> AddDonor(RegisterModel.RegisterInputModel input)
        {
            input.Name = input.Name.ToTitleCase();
            input.Surname = input.Surname.ToTitleCase();
            using (var transaction = _context.Database.BeginTransaction())
            {
                var user = _mapper.Map<User>(input);
                user.Id = Guid.NewGuid();
                var donor = _mapper.Map<Donor>(input);

                try
                {
                    var result = await _userManager.CreateAsync(user, input.Password);
                    if (result.Succeeded)
                    {
                        var addToRoleResult = await _userManager.AddToRoleAsync(user, "Donor");
                        if (addToRoleResult.Succeeded)
                        {
                            donor.DonorId = user.Id;
                            await _donorsService.AddDonor(donor);
                            transaction.Commit();
                            await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                        else
                            return addToRoleResult;
                    }
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return IdentityResult.Failed();
                }
            }
        }

        public async Task<IdentityResult> AddHospitalAdmin(RegisterMedicalStaffInputModel input)
        {
            input.Name = input.Name.ToTitleCase();
            input.Surname = input.Surname.ToTitleCase();
            using (var transaction = _context.Database.BeginTransaction())
            {
                var hospital = await _context.Hospitals
                           .Where(x => x.HospitalId == input.HospitalId)
                           .FirstOrDefaultAsync();

                if (!hospital.HospitalCode.Equals(input.HospitalCode))
                {
                    return IdentityResult.Failed(new IdentityError { Description = "This code doesn't belong to the hospital you entered" });
                }

                var user = _mapper.Map<User>(input);
                user.Id = Guid.NewGuid();
                var medicalStaff = _mapper.Map<MedicalStaff>(input);

                try
                {
                    var result = await _userManager.CreateAsync(user, input.Password);
                    if (result.Succeeded)
                    {
                        var addToRoleResult = await _userManager.AddToRoleAsync(user, "HospitalAdmin");
                        if (addToRoleResult.Succeeded)
                        {
                            medicalStaff.MedicalStaffId = user.Id;
                            await _medicalStaffService.AddMedicalStaff(medicalStaff);
                            transaction.Commit();
                            await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                        else
                            return addToRoleResult;
                    }
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return IdentityResult.Failed();
                }
            }
        }

        public async Task<bool> UserIsInRole(User user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

    }
}