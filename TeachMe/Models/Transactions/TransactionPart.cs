namespace TeachMe.Models.Transactions
{
    public class TransactionPart
    {
        private TransactionAccount account;

        public TransactionPartType Type { get; set; }
        public TransactionAccount Account { get { return account ?? (account = new TransactionAccount()); } set { account = value; } }
        public double Amount { get; set; }
    }
}