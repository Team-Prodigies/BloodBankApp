using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendNotificationToDonors(DonationPost post);
        Task<List<Notification>> GetNotificationsForUser(string userId);
        Task<bool> SendNotificationToDonors(BloodReserveModel reserve, Guid hospitalId);
    }
}