using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        public async Task GetChatConversation(string donorId, string hospitalId)          
        {
         
            var messages = _context.Messages
                .Where(u => u.DonorId == new Guid(donorId) && u.HospitalId == new Guid(hospitalId))
                .OrderBy(t => t.DateSent)
                .Select(m => new {
                    m.MessageId,
                    m.Seen,
                    m.Content,
                    m.DateSent.Hour,
                    m.DateSent.Minute,
                    m.DonorId,
                    m.HospitalId,
                    m.Sender
                }).ToList();

           /* var blogs = _context.Messages
                .FromSqlRaw("SELECT * FROM dbo.Messages")
                .ToList();*/

            string roomName = "ChatRoom-" + donorId + "Donor";
            await JoinRoom(roomName);

            var currentUser = _userManager.GetUserId(Context.User);
            await Clients.User(currentUser).SendAsync("loadChatConversation", messages, donorId, hospitalId);            
        }

        public async Task SendMessage()
        {

        }

        public async Task SendMessages(string content, string donorId, string hospitalId, int sender)
        {

            var newMessage = new Message();
            newMessage.Content = content;
            newMessage.DonorId = new Guid(donorId);
            newMessage.HospitalId = new Guid(hospitalId);
            newMessage.DateSent = DateTime.Now;
            newMessage.Sender = (Enums.MessageSender)sender;
            await _context.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            var resendMessage = new {
                MessageId = newMessage.MessageId,
                Seen = newMessage.Seen,
                Content = newMessage.Content,
                Hour = newMessage.DateSent.Hour,
                Minute = newMessage.DateSent.Minute,
                DonorId = donorId,
                HospitalId = hospitalId,
                Sender = sender
            };

            string roomName = "ChatRoom-" + donorId + "Donor";

            await Clients.Group(roomName).SendAsync("recieveMessage", resendMessage);
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
        public async Task Typing(string donorId)
        {
            string roomName = "ChatRoom-" + donorId + "Donor";
            await Clients.Group(roomName).SendAsync("typing");
        }

        public async Task NotTyping(string donorId) {
            string roomName = "ChatRoom-" + donorId + "Donor";
            await Clients.Group(roomName).SendAsync("notTyping");
        }

        public string GetConnectionId()
        {        
            return Context.ConnectionId;
        }

        public async Task GetWaitingDonors(string hospitalId)
        {
            var currentUser = _userManager.GetUserId(Context.User);

             var donors = await  _context.Messages
                 .Include(user => user.Donor.User)
                 .Where(h=> h.HospitalId == new Guid(hospitalId))
                 .Select(u => new {
                     u.DonorId,
                     u.HospitalId,
                     u.Donor.User.Name,
                     u.Donor.User.Surname
                 }).ToListAsync();
             
            await Clients.User(currentUser).SendAsync("loadWaitingDonors", donors);
        }
    }
}
