using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TeachMe.Models.Payouts;

namespace TeachMe.ViewModels.Payouts
{
    public class PayoutDiscardViewModel
    {
        public PayoutDiscardViewModel()
        {
            
        }

        public PayoutDiscardViewModel(Payout payout)
        {
            Id = payout.Id;
            AdminComment = payout.AdminComment;
        }

        public Guid Id { get; set; }

        [Required]
        [DisplayName("Комментарий")]
        public string AdminComment { get; set; }
    }
}