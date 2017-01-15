using NUnit.Framework;
using System;
using System.Security.Cryptography.X509Certificates;
using TeachMe.Helpers.Payments.Robokassa;

namespace TeachMe.Tests.Helpers.Payments.Robokassa
{
    [TestFixture]
    public class RobokassaPaymentHelperTests
    {
        [Test]
        public void AssertSignatureIsValid_WhenSignatureIsValid_NoExceptions()
        {
            RobokassaPaymentHelper.AssertSignatureIsValid("400.000000", "24", "1ed488ceebb25e8ad362d6704b887fdc", "RobokassaPassword");
        }

        [Test]
        public void AssertSignatureIsValid_WhenSignatureIsInvalid_ThrowException()
        {
            Assert.Throws<Exception>(
                () => RobokassaPaymentHelper.AssertSignatureIsValid("400.000000", "24", "InvalidSignature", "RobokassaPassword"),
                "Bad sign InvalidSignature for amount=400.000000, invoiceId=24");
    }
    }
}
