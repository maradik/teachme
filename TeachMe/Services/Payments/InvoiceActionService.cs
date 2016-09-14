using System;
using Microsoft.AspNet.Identity;
using TeachMe.DataAccess.Payments;
using TeachMe.DataAccess.Transactions;
using TeachMe.Models.Payments;
using TeachMe.Models.Transactions;
using TeachMe.Services.UserCasheSupport;

namespace TeachMe.Services.Payments
{
    public class InvoiceActionService : IInvoiceActionService
    {
        private readonly IInvoiceRepository invoiceRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IUserCashOperationService userCashOperationService;

        public InvoiceActionService(IInvoiceRepository invoiceRepository,
                                    ITransactionRepository transactionRepository,
                                    IUserCashOperationService userCashOperationService)
        {
            this.invoiceRepository = invoiceRepository;
            this.transactionRepository = transactionRepository;
            this.userCashOperationService = userCashOperationService;
        }

        public Invoice CreateInvoice(double amount, string userId, string description)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Должно быть положительным числом");

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var invoice = new Invoice
            {
                Amount = amount,
                UserId = userId,
                Description = description,
                Status = InvoiceStatus.Open
            };

            invoiceRepository.Write(invoice);

            return invoice;
        }

        public void PayInvoice(int invoiceId)
        {
            var invoice = invoiceRepository.Get(invoiceId);

            if (invoice.Status != InvoiceStatus.Open)
                throw new InvalidOperationException($"Невозможно оплатить накладную {invoiceId}, т.к. она уже оплачена");

            invoice.Status = InvoiceStatus.Paid;
            invoiceRepository.Write(invoice);

            WriteTransactionFor(invoice);

            userCashOperationService.AddMoneyToUser(invoice.UserId, invoice.Amount);
        }

        private void WriteTransactionFor(Invoice invoice)
        {
            var transaction = TransactionBuilder.With(TransactionType.InvoicePayment)
                                                .SetText(invoice.Description)
                                                .AddPart(TransactionPartType.Debit, invoice.Amount, new TransactionAccount {UserId = invoice.UserId})
                                                .AddPart(TransactionPartType.Credit, invoice.Amount, new TransactionAccount {InvoiceId = invoice.Id})
                                                .Build();

            transactionRepository.Write(transaction);
        }
    }
}