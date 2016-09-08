using TeachMe.Models.Transactions;

namespace TeachMe.DataAccess.Transactions
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(TransactionRepositoryParameters parameters) : base(parameters)
        {
        }
    }
}