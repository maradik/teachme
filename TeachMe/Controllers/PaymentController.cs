using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using TeachMe.Extensions;
using TeachMe.Helpers.Settings;
using TeachMe.ProjectsSupport;
using TeachMe.Services.Payments;

namespace TeachMe.Controllers
{
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IInvoiceActionService invoiceActionService;

        public PaymentController(IProjectTypeProvider projectTypeProvider,
                                 IInvoiceActionService invoiceActionService)
            : base(projectTypeProvider)
        {
            this.invoiceActionService = invoiceActionService;
        }

        public ActionResult Pay(double amount)
        {
            var invoice = invoiceActionService.CreateInvoice(amount, ApplicationUser.Id, "Пополнение лицевого счета");

            return Redirect(BuildRobokassaPaymentUrl(
                ApplicationSettings.RobokassaLogin,
                ApplicationSettings.RobokassaPassword1,
                invoice.Amount,
                invoice.Id,
                invoice.Description,
                ApplicationSettings.RobokassaIsInTest));
        }

        public ActionResult Result()
        {
            throw new NotImplementedException();
        }

        public ActionResult Success()
        {
            throw new NotImplementedException();
        }

        public ActionResult Fail()
        {
            throw new NotImplementedException();
        }

        private string BuildRobokassaPaymentUrl(string robokassaLogin, string robokassaPassword1, double amount, int invoiceId, string description, bool testPayment)
        {
            if (string.IsNullOrEmpty(robokassaLogin))
                throw new ArgumentException("Value cannot be null or empty.", nameof(robokassaLogin));

            if (string.IsNullOrEmpty(robokassaPassword1))
                throw new ArgumentException("Value cannot be null or empty.", nameof(robokassaPassword1));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (invoiceId <= 0)
                throw new ArgumentOutOfRangeException(nameof(invoiceId));

            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("Value cannot be null or empty.", nameof(description));

            var amountString = amount.ToString("0.00", CultureInfo.InvariantCulture);

            var paymentHash = $"{robokassaLogin}:{amountString}:{invoiceId}:{robokassaPassword1}".GetMd5Hash();

            return string.Format("https://auth.robokassa.ru/Merchant/Index.aspx?MrchLogin={0}&OutSum={1}&InvId={2}&Desc={3}&SignatureValue={4}&isTest={5}&Culture=ru",
                                 Url.Encode(robokassaLogin),
                                 amountString,
                                 invoiceId,
                                 Url.Encode(description),
                                 paymentHash,
                                 testPayment ? "1" : "0");
        }
    }
}
 