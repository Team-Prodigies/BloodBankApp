using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public ChatHub(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SendMessage()
        {
            var currentUser = _userManager.GetUserId(Context.User);
            var user = await _userManager.GetUserAsync(Context.User);

            await Clients.User(currentUser).SendAsync("Client", "Hello "+user.UserName);
           
        }

        public string GetConnectionId()
        {        
            return Context.ConnectionId;
        }

    }
}
