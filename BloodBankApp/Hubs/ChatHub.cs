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

        public async Task GetChatConversation(string recipientId)          
        {
            var currentUser = _userManager.GetUserId(Context.User);
            var user = await _userManager.GetUserAsync(Context.User);

            var recipientFullName = _context.Users
                .Where(u => u.Id == new Guid(recipientId))
                .Select(u => u.Name + " " + u.Surname)
                .FirstOrDefault();

            var messages = _context.Messages
                .Where(u => u.SenderId == user.Id && u.ReceiverId == new Guid(recipientId)
                || u.SenderId == new Guid(recipientId) && u.ReceiverId == user.Id)
                .OrderBy(t => t.DateSent)
                .Select(m => new {
                    m.MessageId,
                    m.Seen,
                    m.Content,
                    m.DateSent.Hour,
                    m.DateSent.Minute,
                    m.SenderId,
                    m.ReceiverId
                }).ToList();
           
            await Clients.User(currentUser).SendAsync("loadChatConversation", messages, recipientId, recipientFullName); 
            
        }
        public async Task SendMessage()
        {
       
        }

        public async Task SendMessages(string content, string recipientId)
        {
            var currentUser = _userManager.GetUserId(Context.User);

            var newMessage = new Message();
            newMessage.Content = content;
            newMessage.SenderId = new Guid(currentUser);
            newMessage.ReceiverId = new Guid(recipientId);
            newMessage.DateSent = DateTime.Now;

            await _context.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            var resendMessage = new {
                MessageId = newMessage.MessageId,
                Seen = newMessage.Seen,
                Content = newMessage.Content,
                Hour = newMessage.DateSent.Hour,
                Minute = newMessage.DateSent.Minute,
                SenderId = newMessage.SenderId,
                ReceiverId = newMessage.ReceiverId
            };


            await Clients.User(currentUser).SendAsync("recieveMessage", resendMessage, 0);
            await Clients.User(recipientId).SendAsync("recieveMessage", resendMessage, 1);
        }

        public string GetConnectionId()
        {        
            return Context.ConnectionId;
        }

        public async Task GetWaitingDonors()
        {
            var currentUser = _userManager.GetUserId(Context.User);

             var donors = await  _context.Messages
                 .Include(user => user.Sender)
                 .Where(m => m.ReceiverId == new Guid(currentUser) && m.Seen == false)
                 .Select(u => new {
                     u.SenderId,
                     u.Sender.Name,
                     u.Sender.Surname
                 }).ToListAsync();

       

            await Clients.User(currentUser).SendAsync("loadWaitingDonors", donors);
        }
    }
}
