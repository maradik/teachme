using System;
using TeachMe.Models.Users;

namespace TeachMe.Services.Payouts
{
    public interface IPayoutActionService
    {
        void CreatePayout(double amount, string qiwiPhoneNumber, ApplicationUser user);
        void DiscardPayout(Guid id, string admincomment);
        void PerformPayout(Guid id);
    }
}