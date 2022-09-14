using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using System.Security.Claims;
using BloodBankApp.ExtensionMethods;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace BloodBankApp.Areas.HospitalAdmin.Services {
    public class HospitalAdminService : IHospitalAdminService {

        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyfService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HospitalAdminService(
           ApplicationDbContext context,
           INotyfService notyfService,
           UserManager<User> userManager,
           IMapper mapper,
           IHttpContextAccessor httpContextAccessor) 
            {
            _context = context;
            _notyfService = notyfService;
            _userManager = userManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            }

        public async Task<IdentityResult> EditHospitalAdmin(HospitalAdminModel hospitalModel) {
            var getUser = await GetUser(_httpContextAccessor.HttpContext.User);
            getUser.Name= hospitalModel.Name;
            getUser.Surname = hospitalModel.Surname;
            getUser.UserName = hospitalModel.UserName;
            getUser.NormalizedUserName = hospitalModel.UserName.ToTitleCase();
            getUser.DateOfBirth = hospitalModel.DateOfBirth;
            getUser.PhoneNumber = hospitalModel.PhoneNumber;

            var result = await _userManager.UpdateAsync(getUser);
            return result;
        }

        public async Task<User> GetUser(ClaimsPrincipal principal) {
            return await _userManager.GetUserAsync(principal);
        }
    }
}
