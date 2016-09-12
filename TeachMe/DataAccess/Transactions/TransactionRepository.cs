using System;
using TeachMe.Models.Transactions;

namespace TeachMe.DataAccess.Transactions
{
    public class TransactionRepository : RepositoryBase<Transaction, Guid>, ITransactionRepository
    {
        public TransactionRepository(TransactionRepositoryParameters parameters) : base(parameters)
        {
        }

        protected override Guid CreateNewId()
        {
            return Guid.NewGuid();
        }
    }
}