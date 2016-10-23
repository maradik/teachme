using TeachMe.Models.Payouts;

namespace TeachMe.ViewModels.Payouts
{
    public class PayoutIndexViewModel
    {
        private Payout[] payouts;

        public bool JustCreated { get; set; }
        public Payout[] Payouts { get { return payouts ?? (payouts = new Payout[0]); } set { payouts = value; } }
    }
}