using System;
using TeachMe.Models.Transactions;

namespace TeachMe.DataAccess.Transactions
{
    public interface ITransactionRepository
    {
        void Write(Transaction model);
        Transaction Get(Guid id);
        void Remove(Guid id);
    }
}