using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BloodBankApp.Hubs
{
    public class NotificationsHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly INotificationService _notificationService;
        private readonly IHospitalAdminService _hospitalAdminService;
        public NotificationsHub(INotificationService notificationService, UserManager<User> userManager, IHospitalAdminService hospitalAdminService)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _hospitalAdminService = hospitalAdminService;
        }

        public async Task GetUnSeenMessagesFromDonor()
        { 
            var currentUser = _userManager.GetUserId(Context.User);
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notificationMessages = await _notificationService.GetUnSeenDonorMessages(new Guid(userId));
            await Clients.User(currentUser).SendAsync("showMessagesNotifications", notificationMessages);
        }

        public async Task GetUnSeenMessagesFromHospital()
        {
            var currentUser = _userManager.GetUserId(Context.User);

            var hospital = await _hospitalAdminService.GetHospitalIdFromHospitalAdmin(new Guid(currentUser));

            var notificationMessages = await _notificationService.GetUnSeenHospitalMessages(hospital);
            await Clients.User(currentUser).SendAsync("showMessagesNotificationsFromDonors", notificationMessages);
        }

        public async Task GetPostsNotifications()
        {
            var currentUser = _userManager.GetUserId(Context.User);
            var postsNotifications = await _notificationService.GetNotificationsForUser(currentUser);

            await Clients.User(currentUser).SendAsync("showPostsNotifications", postsNotifications);
        }
    }
}