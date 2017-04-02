using System;
using TeachMe.Extensions;
using TeachMe.Helpers.Settings;
using TeachMe.Models.Jobs;

namespace TeachMe.Services.Jobs
{
    public class JobReservingRemainAmountSpecification : IJobReservingRemainAmountSpecification
    {
        public static IJobReservingRemainAmountSpecification Instance = new JobReservingRemainAmountSpecification();

        public bool IsSatisfiedBy(Job job)
        {
            return job.GetStudentUser().Cash.AvailableAmount >= job.GetStudentRemainAmount();
        }
    }
}