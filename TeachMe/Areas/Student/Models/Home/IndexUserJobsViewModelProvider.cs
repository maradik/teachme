using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.References;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexUserJobsViewModelProvider
    {
        private IJobRepository jobRepository;
        private ISubjectReference subjectReference;

        public IndexUserJobsViewModelProvider(IJobRepository jobRepository, ISubjectReference subjectReference)
        {
            this.jobRepository = jobRepository;
            this.subjectReference = subjectReference;
        }

        public IndexUserJobViewModel[] Get(ApplicationUser user, int count = 10)
        {
            var jobs = jobRepository.Get(x => x.StudentUserId == user.Id, x => x.CreationTicks, SortOrder.Descending, count);

            return jobs.Select(x => new IndexUserJobViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Status = x.Status,
                Cost = x.StudentCost,
                SubjectTitle = subjectReference.Get(x.SubjectId).Title
            }).ToArray();
        }
    }
}