﻿using System;
using TeachMe.Extensions;
using TeachMe.Models.Jobs;

namespace TeachMe.Services.Jobs
{
    public class JobOpeningSpecification : IJobOpeningSpecification
    {
        public static IJobOpeningSpecification Instance = new JobOpeningSpecification();

        public bool IsSatisfiedBy(Job job)
        {
            return job.GetStudentUser().Cash.AvailableAmount >= job.StudentCost;
        }
    }
}