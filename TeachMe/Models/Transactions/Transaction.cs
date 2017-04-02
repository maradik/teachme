using System;

namespace TeachMe.Models.Transactions
{
    public class Transaction : IEntity<Guid>
    {
        private TransactionPart[] parts;

        public Guid Id { get; set; }
        public TransactionPart[] Parts { get { return parts ?? (parts = new TransactionPart[0]); } set { parts = value; } }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public string Text { get; set; }
        public bool Reverted { get; set; }
        public long CreationTicks { get; set; }
    }
}