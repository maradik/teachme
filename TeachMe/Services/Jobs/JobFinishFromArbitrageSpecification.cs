using System;
using TeachMe.Extensions;
using TeachMe.Helpers.Settings;
using TeachMe.Models.Jobs;

namespace TeachMe.Services.Jobs
{
    public class JobFinishFromArbitrageSpecification : IJobFinishFromArbitrageSpecification
    {
        public static IJobFinishFromArbitrageSpecification Instance = new JobFinishFromArbitrageSpecification();

        public bool IsSatisfiedBy(Job job)
        {
            return job.GetStudentRemainAmount() > 0 && !job.PaymentState.HasFlag(JobPaymentState.RemainReserved);
        }
    }
}