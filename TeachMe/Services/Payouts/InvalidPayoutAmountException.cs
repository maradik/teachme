using System;

namespace TeachMe.Services.Payouts
{
    public class InvalidPayoutAmountException : PayoutActionException
    {
        public InvalidPayoutAmountException(string message) 
            : base(message)
        {
        }
    }
}