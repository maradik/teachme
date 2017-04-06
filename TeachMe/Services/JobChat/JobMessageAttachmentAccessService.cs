using TeachMe.DataAccess.Jobs;
using TeachMe.Extensions;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;

namespace TeachMe.Services.JobChat
{
    public class JobMessageAttachmentAccessService : IJobMessageAttachmentAccessService
    {
        private readonly IJobRepository jobRepository;

        public JobMessageAttachmentAccessService(IJobRepository jobRepository)
        {
            this.jobRepository = jobRepository;
        }

        public bool HasAccess(JobMessage message, ApplicationUser user)
        {
            if (user.IsAdmin())
            {
                return true;
            }
            
            var job = jobRepository.Get(message.JobId);

            if (user.IsTeacher() && job.TeacherUserId == user.Id)
            {
                return true;
            }

            return user.IsStudent() && job.StudentUserId == user.Id && job.Status != JobStatus.FinishedWithRemainAmountNeeded;
        }
    }
}