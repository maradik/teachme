using TeachMe.Helpers.Settings;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.Services.Notifications;
using TeachMe.Extensions;
using TeachMe.DataAccess.Jobs;

namespace TeachMe.Services.Jobs.JobActionHandlers
{
    public class JobMessageCreationOnActionHandler : IJobActionCustomHandler
    {
        private readonly IJobMessageRepository jobMessageRepository;

        public JobMessageCreationOnActionHandler(IJobMessageRepository jobMessageRepository)
        {
            this.jobMessageRepository = jobMessageRepository;
        }

        public void Handle(Job job, JobActionType actionType, ApplicationUser user)
        {
            if (actionType == JobActionType.Edit || actionType == JobActionType.Delete)
            {
                return;
            }

            var jobMessage = new JobMessage
            {
                JobId = job.Id,
                Text = "Пользователь выполнил действие: " + actionType.GetHumanAnnotation(),
                UserId = user.Id
            };

            jobMessageRepository.Write(jobMessage);
        }
    }
}