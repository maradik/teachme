using System;
using System.Collections.Generic;
using System.Linq;

namespace TeachMe.Models.Transactions
{
    public class TransactionBuilder
    {
        private readonly TransactionType transactionType;
        private List<TransactionPart> transactionParts = new List<TransactionPart>();
        private string transactionText;
        private bool revertedTransaction;

        private TransactionBuilder(TransactionType transactionType)
        {
            this.transactionType = transactionType;
        }

        public static TransactionBuilder With(TransactionType type)
        {
            return new TransactionBuilder(type);
        }

        public TransactionBuilder SetText(string text)
        {
            transactionText = text;
            return this;
        }

        public TransactionBuilder AddPart(TransactionPartType type, double amount, TransactionAccount account = null)
        {
            transactionParts.Add(new TransactionPart {Type = type, Amount = amount, Account = account});
            return this;
        }

        public TransactionBuilder Revert()
        {
            revertedTransaction = !revertedTransaction;
            transactionParts = transactionParts.Select(RevertTransactionPart).ToList();
            return this;
        }

        public Transaction Build()
        {
            var transaction = new Transaction
            {
                Type = transactionType,
                Text = transactionText,
                Parts = transactionParts.ToArray(),
                Amount = transactionParts.Where(x => x.Type == TransactionPartType.Credit).Sum(x => x.Amount),
                Reverted = revertedTransaction
            };

            AssertTransactionIsValid(transaction);

            return transaction;
        }

        private TransactionPart RevertTransactionPart(TransactionPart transactionPart)
        {
            return new TransactionPart
            {
                Amount = -transactionPart.Amount,
                Account = transactionPart.Account,
                Type = transactionPart.Type
            };
        }

        private void AssertTransactionIsValid(Transaction transaction)
        {
            var debitWithCommission = transaction.Parts.Where(x => x.Type == TransactionPartType.Debit || x.Type == TransactionPartType.Commission).Sum(x => x.Amount);
            var credit = transaction.Parts.Where(x => x.Type == TransactionPartType.Credit).Sum(x => x.Amount);

            if ((int) (debitWithCommission - credit)*100 != 0)
                throw new Exception($"Некорректный объект {nameof(Transaction)}, т.к. дебит {debitWithCommission} != кредиту {credit}");
        }
    }
}