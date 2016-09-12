using TeachMe.Models.Payments;

namespace TeachMe.Services.Payments
{
    public interface IInvoiceActionService
    {
        Invoice CreateInvoice(double amount, string userId, string description);
        void PayInvoice(int invoiceId);
    }
}