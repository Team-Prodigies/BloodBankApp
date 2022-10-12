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
        private readonly IMessagesService _messagesService;

        public NotificationsHub(IMessagesService messagesService, UserManager<User> userManager)
        {
            _messagesService = messagesService;
            _userManager = userManager;
        }

        public async Task GetUnSeenMessagesFromDonor()
        {
            var currentUser = _userManager.GetUserId(Context.User);
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notificationMessages = await _messagesService.GetUnSeenDonorMessages(new Guid(userId));
            await Clients.User(currentUser).SendAsync("showMessagesNotifications", notificationMessages);
        }
    }
}
