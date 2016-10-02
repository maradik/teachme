using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.Models.Jobs;

namespace TeachMe.Services.Jobs
{
    public class JobDeletionSpecification : ISpecification<Job>, IJobDeletionSpecification
    {
        public static IJobDeletionSpecification Instance = new JobDeletionSpecification();

        public bool IsSatisfiedBy(Job job)
        {
            return string.IsNullOrEmpty(job.TeacherUserId);
        }
    }
}