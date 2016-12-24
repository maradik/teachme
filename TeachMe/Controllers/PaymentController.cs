using log4net;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using TeachMe.Extensions;
using TeachMe.Helpers.Settings;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;
using TeachMe.Services.Payments;

namespace TeachMe.Controllers
{
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PaymentController));
        private readonly IInvoiceActionService invoiceActionService;

        public PaymentController(IProjectTypeProvider projectTypeProvider,
                                 IProjectInfoProvider projectInfoProvider,
                                 IInvoiceActionService invoiceActionService)
            : base(projectTypeProvider, projectInfoProvider)
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

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SetResult(string outSum, string invId, string signatureValue)
        {
            try
            {
                var amount = double.Parse(outSum, CultureInfo.InvariantCulture);
                var invoiceId = int.Parse(invId, CultureInfo.InvariantCulture);

                AssertSignatureIsValid(amount, invoiceId, signatureValue, ApplicationSettings.RobokassaPassword2);

                invoiceActionService.PayInvoice(invoiceId);

                Logger.Info($"Уведомление о платеже на сумму {outSum} по счету {invId}");
                return Content($"OK{invId}");
            }
            catch (Exception e)
            {
                Logger.Error($"Ошибка при получении уведомления о платеже на сумму {outSum} по счету {invId}", e);
                throw;
            }
        }

        public ActionResult ShowSuccess(double outSum, int invId, string signatureValue)
        {
            try
            {
                AssertSignatureIsValid(outSum, invId, signatureValue, ApplicationSettings.RobokassaPassword1);
                Logger.Info($"Удачный платеж на сумму {outSum} по счету {invId}");
            }
            catch (Exception e)
            {
                Logger.Error($"Ошибка при отображении страницы удачного платежа на сумму {outSum} по счету {invId}", e);
                throw;
            }

            return View();
        }

        public ActionResult ShowFail(double outSum, int invId, string signatureValue)
        {
            Logger.Warn($"Неудачный платеж на сумму {outSum} по счету {invId}");
            return View();
        }

        private static void AssertSignatureIsValid(double amount, int invoiceId, string signatureValue, string robokassaPassword)
        {
            string reconstructedSignatureValue = $"{amount.ToString("0.00", CultureInfo.InvariantCulture)}:{invoiceId}:{robokassaPassword}";

            if (!string.Equals(reconstructedSignatureValue.GetMd5Hash(), signatureValue, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Bad sign {signatureValue} for {nameof(amount)}={amount}, {nameof(invoiceId)}={invoiceId}");
            }
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
 