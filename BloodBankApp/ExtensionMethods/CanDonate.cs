using BloodBankApp.Models;
using System.Threading.Tasks;
using System;
using BloodBankApp.Areas.HospitalAdmin.Services;

namespace BloodBankApp.ExtensionMethods
{
    public class CanDonate
    {
        private readonly DonatorService _donatorService;
        public CanDonate(DonatorService donatorService)
        {
            _donatorService = donatorService;
        }

        public int MonthsPassed(DateTime lastDonationDate)
        {
            var months = (DateTime.Now.Year - lastDonationDate.Year) * 12;
            months = months + DateTime.Now.Month - lastDonationDate.Month;

            if (DateTime.Now.Day < lastDonationDate.Day)
            {
                months--;
            }
            return months;
        }

        public bool CanDonateAgain(Donor donor, int months)
        {
            if (donor.Gender == Enums.Gender.MALE)
            {
                return (months >= 3);
            }
            else
            {
                return (months >= 4);
            }
        }
    }
}
