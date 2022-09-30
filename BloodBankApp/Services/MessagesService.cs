using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        public MessagesService( ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<SendMessage>> GetChatConversation(string donorId, string hospitalId)
        {
            var messages = await _context.Messages
               .Where(u => u.DonorId == new Guid(donorId) && u.HospitalId == new Guid(hospitalId))
               .OrderBy(t => t.DateSent)
               .Select(m => new SendMessage {
                   MessageId = m.MessageId,
                   Seen = m.Seen,
                   Content = m.Content,
                   Hour = m.DateSent.Hour.ToString(),
                   Minute = m.DateSent.Minute.ToString(),
                   DonorId = new Guid(donorId),
                   HospitalId = new Guid(hospitalId),
                   Sender = m.Sender
               }).ToListAsync();

            return messages;
        }

        public async Task<List<WaitingDonor>> GetWaitingDonors(string hospitalId)
        {
            var waitingDonors = await _context.Messages
                .Include(user => user.Donor.User)
                .Where(h => h.HospitalId == new Guid(hospitalId))
                .Select(u => new WaitingDonor{
                    DonorId = u.DonorId,
                    HospitalId = u.HospitalId,
                    Name = u.Donor.User.Name,
                    Surname = u.Donor.User.Surname
                })
                .Distinct()
                .ToListAsync();

            return waitingDonors;
        }

        public async Task<SendMessage> SaveMessage(string content, string donorId, string hospitalId, int sender)
        {
            var newMessage = new Message(DateTime.Now, content, new Guid(donorId), new Guid(hospitalId), sender);
            await _context.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            var sendMessage = new SendMessage{
                MessageId = newMessage.MessageId,
                Seen = newMessage.Seen,
                Content = newMessage.Content,
                Hour = newMessage.DateSent.Hour.ToString(),
                Minute = newMessage.DateSent.Minute.ToString(),
                DonorId = new Guid(donorId),
                HospitalId = new Guid(hospitalId),
                Sender = (MessageSender)sender
            };

            return sendMessage;
        }

        public async Task SetDonorMessagesToSeen(string donorId, string hospitalId)
        {
            try {
                var query = $"UPDATE Messages SET Seen = 'true' WHERE DonorId = '{donorId}' AND HospitalId = '{hospitalId}' AND Sender = 0 AND Seen = 'false'";
                await _context.Database.ExecuteSqlRawAsync(query);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public async Task SetHospitalMessagesToSeen(string donorId, string hospitalId)
        {
            try {
                var query = $"UPDATE Message SET Seen = 'true' WHERE DonorId = '{donorId}' AND HospitalId = '{hospitalId}' AND Sender = 1 AND Seen = 'false'";
                await _context.Database.ExecuteSqlRawAsync(query);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public async Task SetMessageToSeen(string messageId)
        {
            var message = await _context.Messages.FindAsync(new Guid(messageId));

            if(message != null)
            {
                message.Seen = true;
                _context.Messages.Update(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteChat(string donorId, string hospitalId)
        {
            try {
                var query = $"DELETE FROM Messages WHERE DonorId = '{donorId}' AND HospitalId = '{hospitalId}'";
                await _context.Database.ExecuteSqlRawAsync(query);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
}
