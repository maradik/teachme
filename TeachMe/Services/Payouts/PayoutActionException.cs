using System;

namespace TeachMe.Services.Payouts
{
    public class PayoutActionException : Exception
    {
        public PayoutActionException(string message)
            : base(message)
        {
            
        }
    }
}