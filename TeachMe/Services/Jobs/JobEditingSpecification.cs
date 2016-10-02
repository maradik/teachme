using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.Models.Jobs;

namespace TeachMe.Services.Jobs
{
    public class JobEditingSpecification : ISpecification<Job>, IJobEditingSpecification
    {
        public static IJobEditingSpecification Instance = new JobEditingSpecification();

        public bool IsSatisfiedBy(Job job)
        {
            return string.IsNullOrEmpty(job.TeacherUserId);
        }
    }
}