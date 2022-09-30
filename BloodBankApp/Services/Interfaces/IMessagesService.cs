using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Services.Interfaces
{
    public interface IMessagesService
    {
        public Task<List<SendMessage>> GetChatConversation(string donorId, string hospitalId);

        public Task<SendMessage> SaveMessage(string content, string donorId, string hospitalId, int sender);

        public Task<List<WaitingDonor>> GetWaitingDonors(string hospitalId);

        public Task SetDonorMessagesToSeen(string donorId, string hospitalId);
        public Task SetHospitalMessagesToSeen(string donorId, string hospitalId);

        public Task SetMessageToSeen(string messageId);

        public Task DeleteChat(string donorId, string hospitalId);
    }
}
