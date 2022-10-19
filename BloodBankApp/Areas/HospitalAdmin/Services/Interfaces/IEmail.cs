using BloodBankApp.Areas.HospitalAdmin.Model;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces {
    public interface IEmail 
    {
        Task<bool> SendEmail(EmailData emailData);
    }
}
