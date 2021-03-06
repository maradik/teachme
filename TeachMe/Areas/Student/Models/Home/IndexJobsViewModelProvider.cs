﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.Models.Jobs;
using TeachMe.References;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexJobsViewModelProvider
    {
        private IJobRepository jobRepository;
        private ISubjectReference subjectReference;

        public IndexJobsViewModelProvider(IJobRepository jobRepository, ISubjectReference subjectReference)
        {
            this.jobRepository = jobRepository;
            this.subjectReference = subjectReference;
        }

        public IndexJobViewModel[] Get(int count = 10)
        {
            var jobStatuses = new[] { JobStatus.Opened, JobStatus.InWorking, JobStatus.InReWorking, JobStatus.Finished, JobStatus.Accepted };

            var jobs = jobRepository.Get(x => jobStatuses.Contains(x.Status), x => x.CreationTicks, SortOrder.Descending, count);

            return jobs.Select(x => new IndexJobViewModel
            {
                Title = x.Title,
                Status = x.Status,
                Cost = x.StudentCost,
                SubjectTitle = subjectReference.Get(x.SubjectId).Title
            }).ToArray();
        }
    }
}