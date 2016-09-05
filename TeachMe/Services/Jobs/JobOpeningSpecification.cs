using System;
using TeachMe.Extensions;
using TeachMe.Models.Jobs;

namespace TeachMe.Services.Jobs
{
    public class JobOpeningSpecification : IJobOpeningSpecification
    {
        private static readonly Lazy<JobOpeningSpecification> lazyInstance = new Lazy<JobOpeningSpecification>(() => new JobOpeningSpecification(), true);

        public static IJobOpeningSpecification Instance => lazyInstance.Value;

        public bool IsSatisfiedBy(Job job)
        {
            return job.GetStudentUser().Cash.AvailableAmount >= job.StudentCost;
        }
    }
}