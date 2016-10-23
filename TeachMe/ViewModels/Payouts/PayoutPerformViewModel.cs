using System;
using TeachMe.Models.Payouts;

namespace TeachMe.ViewModels.Payouts
{
    public class PayoutPerformViewModel
    {
        public PayoutPerformViewModel()
        {
            
        }

        public PayoutPerformViewModel(Payout payout)
        {
            Id = payout.Id;
        }

        public Guid Id { get; set; }
    }
}