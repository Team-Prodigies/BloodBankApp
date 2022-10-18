using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Services.Interfaces
{
    public interface IMessagesService
    {
        public Task<List<SendMessage>> GetChatConversation(Guid donorId, Guid hospitalId);
        public Task<SendMessage> SaveMessage(string content, Guid donorId, Guid hospitalId, int sender);
        public Task<List<WaitingDonor>> GetWaitingDonors(Guid hospitalId);
        public Task SetDonorMessagesToSeen(Guid donorId, Guid hospitalId);
        public Task SetHospitalMessagesToSeen(Guid donorId, Guid hospitalId);
        public Task SetMessageToSeen(Guid messageId);
        public Task DeleteChat(Guid donorId, Guid hospitalId);
    }
}