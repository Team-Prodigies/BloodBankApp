using AutoMapper;
using BloodBankApp.Areas.Identity.Pages.Account;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BloodBankApp.Areas.Identity.Services.Interfaces;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.ExtensionMethods;
using static BloodBankApp.Areas.Identity.Pages.Account.RegisterMedicalStaffModel;
using Microsoft.AspNetCore.Http;

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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(
            IMedicalStaffService medicalStaffService,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IDonorsService donorsService,
            UserManager<User> userManager,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _medicalStaffService = medicalStaffService;
            _signInManager = signInManager;
            _context = context;
            _donorsService = donorsService;
            _userManager = userManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> AddSuperAdmin(SuperAdminModel user)
        {
            user.Name = user.Name.ToTitleCase();
            user.Surname = user.Surname.ToTitleCase();
            var newUser = _mapper.Map<User>(user);

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                return await AddUserToRoles(newUser, user.Roles);
            }
            return result;
        }

        private async Task<IdentityResult> AddUserToRoles(User user, List<SelectedRoleModel> roles)
        {
            foreach (var role in roles)
            {
                if (role.IsSelected && !role.RoleName.Equals("Donor") && !role.RoleName.Equals("HospitalAdmin"))
                {
                    var result = await _userManager.AddToRoleAsync(user, role.RoleName);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> EditSuperAdmin(ProfileAdminModel user)
        {
            user.Name = user.Name.ToTitleCase();
            user.Surname = user.Surname.ToTitleCase();
            var superAdmin = await GetUser(_httpContextAccessor.HttpContext.User);
            superAdmin.Name = user.Name;
            superAdmin.Surname = user.Surname;
            superAdmin.UserName = user.UserName;
            superAdmin.DateOfBirth = user.DateOfBirth;
            superAdmin.Email = user.Email;

            var result = await _userManager.UpdateAsync(superAdmin);

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
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var user = _mapper.Map<User>(input);
            user.Id = Guid.NewGuid();
            var donor = _mapper.Map<Donor>(input);

            try
            {
                var result = await _userManager.CreateAsync(user, input.Password);
                if (!result.Succeeded) return result;
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "Donor");
                if (addToRoleResult.Succeeded)
                {
                    donor.DonorId = user.Id;
                    await _donorsService.AddDonor(donor);
                    await transaction.CommitAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                    return addToRoleResult;
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> AddNonRegisteredDonor(RegisterModel.RegisterInputModel input)
        {
            input.Name = input.Name.ToTitleCase();
            input.Surname = input.Surname.ToTitleCase();

            var donorExists = await _context.Donors
                .Where(donor => donor.User.Name.Equals(input.Name)
                                && donor.BloodTypeId == input.BloodTypeId
                                && donor.PersonalNumber == input.PersonalNumber)
                .FirstOrDefaultAsync();
            if (donorExists == null)
            {
                return IdentityResult.Failed();
            };
            var userExists = await _context.Users
                .Where(user => user.UserName == null)
                .FirstOrDefaultAsync(u => u.Id == donorExists.DonorId);

            if (userExists == null)
            {
                return IdentityResult.Failed();
            };

            var code = await _context.Codes.FirstOrDefaultAsync(x => x.CodeId == donorExists.DonorId);

            var bloodDonations = await _context.BloodDonations
                .FirstOrDefaultAsync(x => x.DonorId == donorExists.DonorId);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            {
                var user = _mapper.Map<User>(input);
                user.Id = Guid.NewGuid();
                var donor = _mapper.Map<Donor>(input);

                try
                {
                    var result = await _userManager.CreateAsync(user, input.Password);
                    if (!result.Succeeded) return result;
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "Donor");
                    if (addToRoleResult.Succeeded)
                    {
                        donor.DonorId = user.Id;
                        await _donorsService.AddDonor(donor);

                        bloodDonations.DonorId = donor.DonorId;

                        var newCode = new Code
                        {
                            CodeId = donor.DonorId,
                            CodeValue = code.CodeValue
                        };
                        await _context.Codes.AddAsync(newCode);
                        _context.Users.Remove(userExists);
                        await _context.SaveChangesAsync();


                        await transaction.CommitAsync();
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                        return addToRoleResult;
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
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

        public async Task<bool> CheckDonorsCode(Guid id, string codeValue)
        {
            var code = await _context.Codes
                .Where(x=>x.CodeValue == codeValue)
                .FirstOrDefaultAsync(code=>code.CodeId == id);
            if(code == null) return false;
            return true;
        }

        public async Task<bool> DonorExists(RegisterModel.RegisterInputModel input)
        {
            input.Name = input.Name.ToTitleCase();
            input.Surname = input.Surname.ToTitleCase();

            var donorExists = await _context.Donors
                .Where(donor => donor.User.Name.Equals(input.Name)
                                && donor.BloodTypeId == input.BloodTypeId
                                && donor.PersonalNumber == input.PersonalNumber)
                .FirstOrDefaultAsync();
            if (donorExists == null)
            {
                return false;
            };
            var userExists = await _context.Users
                .Where(user => user.UserName == null)
                .FirstOrDefaultAsync(u => u.Id == donorExists.DonorId);

            if (userExists == null)
            {
                return false;
            };
            return true;
        }
    }
}