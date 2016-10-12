using System.Linq;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.Models.Jobs;
using TeachMe.References;

namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexJobsViewModelProvider
    {
        private readonly IJobRepository jobRepository;
        private readonly ISubjectReference subjectReference;

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
                Id = x.Id,
                Title = x.Title,
                Status = x.Status,
                Cost = x.TeacherCost,
                SubjectTitle = subjectReference.Get(x.SubjectId).Title,
                CanBeShown = string.IsNullOrEmpty(x.TeacherUserId)
            }).ToArray();
        }
    }
}