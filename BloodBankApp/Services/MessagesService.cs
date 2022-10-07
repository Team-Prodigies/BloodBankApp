using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly ApplicationDbContext _context;
        public MessagesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<SendMessage>> GetChatConversation(Guid donorId, Guid hospitalId)
        {
            var messages = await _context.Messages
               .Where(u => u.DonorId == donorId && u.HospitalId == hospitalId)
               .OrderBy(t => t.DateSent)
               .Select(m => new SendMessage
               {
                   MessageId = m.MessageId,
                   Seen = m.Seen,
                   Content = m.Content,
                   Hour = m.DateSent.Hour.ToString(),
                   Minute = m.DateSent.Minute.ToString(),
                   DonorId = donorId,
                   HospitalId = hospitalId,
                   Sender = m.Sender
               }).ToListAsync();

            return messages;
        }

        public async Task<List<WaitingDonor>> GetWaitingDonors(Guid hospitalId)
        {
            var waitingDonors = await _context.Messages
                .Include(user => user.Donor.User)
                .Where(h => h.HospitalId == hospitalId)
                .Select(u => new WaitingDonor
                {
                    DonorId = u.DonorId,
                    HospitalId = u.HospitalId,
                    Name = u.Donor.User.Name,
                    Surname = u.Donor.User.Surname
                })
                .Distinct()
                .ToListAsync();

            return waitingDonors;
        }

        public async Task<SendMessage> SaveMessage(string content, Guid donorId, Guid hospitalId, int sender)
        {
            var newMessage = new Message(DateTime.Now, content, donorId, hospitalId, sender);
            await _context.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            var sendMessage = new SendMessage
            {
                MessageId = newMessage.MessageId,
                Seen = newMessage.Seen,
                Content = newMessage.Content,
                Hour = newMessage.DateSent.Hour.ToString(),
                Minute = newMessage.DateSent.Minute.ToString(),
                DonorId = donorId,
                HospitalId = hospitalId,
                Sender = (MessageSender)sender
            };

            return sendMessage;
        }

        public async Task SetDonorMessagesToSeen(Guid donorId, Guid hospitalId)
        {
            try
            {
                var query = $"UPDATE Messages SET Seen = 'true' WHERE DonorId = '{donorId}' AND HospitalId = '{hospitalId}' AND Sender = 0 AND Seen = 'false'";
                await _context.Database.ExecuteSqlRawAsync(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task SetHospitalMessagesToSeen(Guid donorId, Guid hospitalId)
        {
            try
            {
                var query = $"UPDATE Message SET Seen = 'true' WHERE DonorId = '{donorId}' AND HospitalId = '{hospitalId}' AND Sender = 1 AND Seen = 'false'";
                await _context.Database.ExecuteSqlRawAsync(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task SetMessageToSeen(Guid messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);

            if (message != null)
            {
                message.Seen = true;
                _context.Messages.Update(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteChat(Guid donorId, Guid hospitalId)
        {
            try
            {
                var query = $"DELETE FROM Messages WHERE DonorId = '{donorId}' AND HospitalId = '{hospitalId}'";
                await _context.Database.ExecuteSqlRawAsync(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}