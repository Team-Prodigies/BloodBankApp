using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendNotificationToDonors(DonationPost post);
        Task<List<Notification>> GetNotificationsForUser(string userId);
        Task<List<Notification>> GetNotificationsForDonor(string donorId);
        Task<bool> SendNotificationToDonors(BloodReserveModel reserve, Guid hospitalId);
        Task<List<MessageNotification>> GetUnSeenDonorMessages(Guid donorId);
        Task<List<DonorModel>> GetUnSeenHospitalMessages(Guid hospitalId);
        Task<List<Donor>> GetPotentialDonors(string bloodTypeName, Guid cityId);
    }
}