using System;
using System.Globalization;
using TeachMe.Extensions;
using System.Web.Mvc;

namespace TeachMe.Helpers.Payments.Robokassa
{
    public static class RobokassaPaymentHelper
    {
        public static void AssertSignatureIsValid(string amount, string invoiceId, string signatureValue, string robokassaPassword)
        {
            string reconstructedSignatureValue = $"{amount}:{invoiceId}:{robokassaPassword}";

            if (!string.Equals(reconstructedSignatureValue.GetMd5Hash(), signatureValue, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Bad sign {signatureValue} for {nameof(amount)}={amount}, {nameof(invoiceId)}={invoiceId}");
            }
        }

        public static string BuildRobokassaPaymentUrl(string robokassaLogin, string robokassaPassword1, double amount, int invoiceId, string description, bool testPayment)
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

            var urlHelper = new UrlHelper();
            return string.Format("https://auth.robokassa.ru/Merchant/Index.aspx?MrchLogin={0}&OutSum={1}&InvId={2}&Desc={3}&SignatureValue={4}&isTest={5}&Culture=ru",
                                 urlHelper.Encode(robokassaLogin),
                                 amountString,
                                 invoiceId,
                                 urlHelper.Encode(description),
                                 paymentHash,
                                 testPayment ? "1" : "0");
        }
    }
}