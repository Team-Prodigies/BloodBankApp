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

        public NotificationsHub(INotificationService notificationService, UserManager<User> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task GetUnSeenMessagesFromDonor()
        { 
            var currentUser = _userManager.GetUserId(Context.User);
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notificationMessages = await _notificationService.GetUnSeenDonorMessages(new Guid(userId));
            await Clients.User(currentUser).SendAsync("showMessagesNotifications", notificationMessages);
        }

        public async Task GetPostsNotifications()
        {
            var currentUser = _userManager.GetUserId(Context.User);
            var postsNotifications = await _notificationService.GetNotificationsForUser(currentUser);

            await Clients.User(currentUser).SendAsync("showPostsNotifications", postsNotifications);
        }
    }
}