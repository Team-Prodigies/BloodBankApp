using BloodBankApp.Data;
using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
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
        private readonly IMessagesService _messagesService;
        public ChatHub(UserManager<User> userManager, IMessagesService messagesService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _messagesService = messagesService;
            _context = context;
        }

        public async Task GetChatConversation(string donorId, string hospitalId)          
        {
            var messages = await _messagesService.GetChatConversation(donorId,hospitalId);

            string roomName = "ChatRoom-" + donorId + "Donor";
            await JoinRoom(roomName);

            var currentUser = _userManager.GetUserId(Context.User);
            await Clients.User(currentUser).SendAsync("loadChatConversation", messages, donorId, hospitalId);            
        }

        public async Task SendMessages(string content, string donorId, string hospitalId, int sender)
        {
            var sendMessage = await _messagesService.SaveMessage(content, donorId, hospitalId, sender);

            string roomName = "ChatRoom-" + donorId + "Donor";

            await Clients.Group(roomName).SendAsync("receiveMessage", sendMessage);
        }

        public async Task SetMessageToSeen(string messageId)
        {
            await _messagesService.SetMessageToSeen(messageId);
        }

        public async Task SetDonorMessagesToSeen(string donorId, string hospitalId)
        {
            await _messagesService.SetDonorMessagesToSeen(donorId, hospitalId);
        }

        public async Task SetHospitalMessagesToSeen(string donorId, string hospitalId)
        {
            await _messagesService.SetHospitalMessagesToSeen(donorId, hospitalId);
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }
        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
        public async Task Typing(string donorId, int typingIndicator)
        {
            string roomName = "ChatRoom-" + donorId + "Donor";
            await Clients.Group(roomName).SendAsync("typing", typingIndicator, donorId);
        }

        public async Task NotTyping(string donorId, int typingIndicator) {
            string roomName = "ChatRoom-" + donorId + "Donor";
            await Clients.Group(roomName).SendAsync("notTyping", typingIndicator);
        }

        public string GetConnectionId()
        {        
            return Context.ConnectionId;
        }
        public async Task GetWaitingDonors(string hospitalId)
        {
            var currentUser = _userManager.GetUserId(Context.User);

            var waitingDonors = await _messagesService.GetWaitingDonors(hospitalId);
          
            await Clients.User(currentUser).SendAsync("loadWaitingDonors", waitingDonors);
        }

        public async Task DeleteChat(string donorId, string hospitalId)
        {
            var currentUser = _userManager.GetUserId(Context.User);
            await _messagesService.DeleteChat(donorId, hospitalId);        
            await Clients.User(currentUser).SendAsync("removeWaitingDonor", donorId);
        }
    }
}
