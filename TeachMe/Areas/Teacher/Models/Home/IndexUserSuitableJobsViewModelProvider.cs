using System.Linq;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.References;

namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexUserSuitableJobsViewModelProvider
    {
        private readonly IJobRepository jobRepository;
        private readonly ISubjectReference subjectReference;

        public IndexUserSuitableJobsViewModelProvider(IJobRepository jobRepository, ISubjectReference subjectReference)
        {
            this.jobRepository = jobRepository;
            this.subjectReference = subjectReference;
        }

        public IndexUserSuitableJobViewModel[] Get(ApplicationUser user, int count = 10)
        {
            var jobs = jobRepository.Get(x => x.Status == JobStatus.Opened && user.SubjectIds.Contains(x.SubjectId), x => x.CreationTicks, SortOrder.Descending, count);

            return jobs.Select(x => new IndexUserSuitableJobViewModel
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