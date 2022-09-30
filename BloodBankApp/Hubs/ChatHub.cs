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
        private readonly IMessagesService _messagesService;
        public ChatHub(UserManager<User> userManager, IMessagesService messagesService)
        {
            _userManager = userManager;
            _messagesService = messagesService;
        }

        public async Task GetChatConversation(Guid donorId, Guid hospitalId)          
        {
            var messages = await _messagesService.GetChatConversation(donorId,hospitalId);

            string roomName = "ChatRoom-" + donorId + "Donor";
            await JoinRoom(roomName);

            var currentUser = _userManager.GetUserId(Context.User);
            await Clients.User(currentUser).SendAsync("loadChatConversation", messages, donorId, hospitalId);            
        }

        public async Task SendMessages(string content, Guid donorId, Guid hospitalId, int sender)
        {
            var sendMessage = await _messagesService.SaveMessage(content, donorId, hospitalId, sender);

            string roomName = "ChatRoom-" + donorId + "Donor";

            await Clients.Group(roomName).SendAsync("receiveMessage", sendMessage);
        }

        public async Task SetMessageToSeen(Guid messageId)
        {
            await _messagesService.SetMessageToSeen(messageId);
        }

        public async Task SetDonorMessagesToSeen(Guid donorId, Guid hospitalId)
        {
            await _messagesService.SetDonorMessagesToSeen(donorId, hospitalId);
        }

        public async Task SetHospitalMessagesToSeen(Guid donorId, Guid hospitalId)
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
        public async Task Typing(Guid donorId, int typingIndicator)
        {
            string roomName = "ChatRoom-" + donorId + "Donor";
            await Clients.Group(roomName).SendAsync("typing", typingIndicator, donorId);
        }

        public async Task NotTyping(Guid donorId, int typingIndicator) {
            string roomName = "ChatRoom-" + donorId + "Donor";
            await Clients.Group(roomName).SendAsync("notTyping", typingIndicator);
        }

        public string GetConnectionId()
        {        
            return Context.ConnectionId;
        }
        public async Task GetWaitingDonors(Guid hospitalId)
        {
            var currentUser = _userManager.GetUserId(Context.User);

            var waitingDonors = await _messagesService.GetWaitingDonors(hospitalId);
          
            await Clients.User(currentUser).SendAsync("loadWaitingDonors", waitingDonors);
        }

        public async Task DeleteChat(Guid donorId, Guid hospitalId)
        {
            var currentUser = _userManager.GetUserId(Context.User);
            string roomName = "ChatRoom-" + donorId + "Donor";
            
            await _messagesService.DeleteChat(donorId, hospitalId);

            await Clients.Group(roomName).SendAsync("deleteChat");
            await Clients.User(currentUser).SendAsync("removeWaitingDonor", donorId);
             
        }
    }
}
