using System.Linq;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.References;

namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexUserJobsViewModelProvider
    {
        private readonly IJobRepository jobRepository;
        private readonly ISubjectReference subjectReference;

        public IndexUserJobsViewModelProvider(IJobRepository jobRepository, ISubjectReference subjectReference)
        {
            this.jobRepository = jobRepository;
            this.subjectReference = subjectReference;
        }

        public IndexUserJobViewModel[] Get(ApplicationUser user, int count = 10)
        {
            var inProgressJobStatuses = new[] {JobStatus.InWorking, JobStatus.InReWorking, JobStatus.Finished, JobStatus.AbortOffered};

            var jobs = jobRepository.Get(x => x.TeacherUserId == user.Id && inProgressJobStatuses.Contains(x.Status), x => x.CreationTicks, SortOrder.Descending, count);

            return jobs.Select(x => new IndexUserJobViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Status = x.Status,
                Cost = x.TeacherCost,
                SubjectTitle = subjectReference.Get(x.SubjectId).Title
            }).ToArray();
        }
    }
}