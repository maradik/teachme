using log4net;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using TeachMe.Extensions;
using TeachMe.Helpers.Payments.Robokassa;
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

            return Redirect(RobokassaPaymentHelper.BuildRobokassaPaymentUrl(
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
                RobokassaPaymentHelper.AssertSignatureIsValid(outSum, invId, signatureValue, ApplicationSettings.RobokassaPassword2);

                var amount = double.Parse(outSum, CultureInfo.InvariantCulture);
                var invoiceId = int.Parse(invId, CultureInfo.InvariantCulture);
                invoiceActionService.PayInvoice(invoiceId);

                Logger.Info($"Уведомление о платеже на сумму {amount} по счету {invoiceId}");
                return Content($"OK{invId}");
            }
            catch (Exception e)
            {
                Logger.Error($"Ошибка при получении уведомления о платеже на сумму {outSum} по счету {invId}", e);
                throw;
            }
        }

        public ActionResult ShowSuccess(string outSum, string invId, string signatureValue)
        {
            try
            {
                RobokassaPaymentHelper.AssertSignatureIsValid(outSum, invId, signatureValue, ApplicationSettings.RobokassaPassword1);
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
    }
}
 