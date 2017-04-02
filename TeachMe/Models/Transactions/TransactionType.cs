namespace TeachMe.Models.Transactions
{
    public enum TransactionType
    {
        Undefined = 0,
        CompleteJobPayment = 1,
        InvoicePayment = 2,
        Payout = 3,
        JobPrepayment = 4
    }
}