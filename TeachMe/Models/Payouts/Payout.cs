using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models.Payouts
{
    public class Payout : IEntity<Guid>
    {
        private PayoutRecipient recipient;

        public Guid Id { get; set; }

        [DisplayName("Пользователь")]
        public string UserId { get; set; }

        [Required]
        [Range(100, 10000)]
        [DisplayName("Сумма")]
        public double Amount { get; set; }

        [DisplayName("Статус")]
        public PayoutStatus Status { get; set; }

        [DisplayName("Получатель")]
        public PayoutRecipient Recipient { get { return recipient ?? (recipient = new PayoutRecipient()); } set { recipient = value; } }

        [DisplayName("Комментарий")]
        public string AdminComment { get; set; }

        [DisplayName("Дата создания")]
        public long CreationTicks { get; set; }
    }
}