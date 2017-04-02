using System;

namespace TeachMe.Models.Jobs
{
    [Flags]
    public enum JobPaymentState
    {
        Default = 0,
        RemainReserved = 1
    }
}