using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.Transactions
{
    public class TransactionRepository : RepositoryBase<Job>
    {
        public TransactionRepository(TransactionRepositoryParameters parameters) : base(parameters)
        {
        }
    }
}