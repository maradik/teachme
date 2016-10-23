using System;

namespace TeachMe.Models.Transactions
{
    public class TransactionAccount
    {
        public string UserId { get; set; }
        public int? InvoiceId { get; set; }
        public Guid? PayoutId { get; set; }
    }
}