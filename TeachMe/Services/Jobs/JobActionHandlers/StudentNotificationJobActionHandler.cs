using TeachMe.Helpers.Settings;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.Services.Notifications;
using TeachMe.Extensions;

namespace TeachMe.Services.Jobs.JobActionHandlers
{
    public class StudentNotificationJobActionHandler : IJobActionCustomHandler
    {
        private readonly ISmsService smsService;

        public StudentNotificationJobActionHandler(ISmsService smsService)
        {
            this.smsService = smsService;
        }

        public void Handle(Job job, JobActionType actionType, ApplicationUser user)
        {
            switch (actionType)
            {
                case JobActionType.Take:
                    NotificateUser(job.GetStudentUser(), "Ваша задача взята в работу.");
                    break;
                case JobActionType.Finish:
                    NotificateUser(job.GetStudentUser(), "Ваша задача выполнена.");
                    break;
            }
        }

        private void NotificateUser(ApplicationUser user, string text)
        {
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                smsService.Send(user.PhoneNumber, text + $" {ApplicationSettings.StudentProjectName}");
            }
        }
    }
}