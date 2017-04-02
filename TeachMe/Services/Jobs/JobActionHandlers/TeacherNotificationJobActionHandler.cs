using TeachMe.Helpers.Settings;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.Services.Notifications;
using TeachMe.Extensions;

namespace TeachMe.Services.Jobs.JobActionHandlers
{
    public class TeacherNotificationJobActionHandler : IJobActionCustomHandler
    {
        private readonly ISmsService smsService;

        public TeacherNotificationJobActionHandler(ISmsService smsService)
        {
            this.smsService = smsService;
        }

        public void Handle(Job job, JobActionType actionType, ApplicationUser user)
        {
            if (string.IsNullOrEmpty(job.TeacherUserId))
                return;

            switch (actionType)
            {
                case JobActionType.AcceptWithoutRemainAmount:
                    NotificateUser(job.GetTeacherUser(), $"Поступила оплата за задачу {(int)job.TeacherPrepaymentAmount} руб.");
                    break;
                case JobActionType.Accept:
                    NotificateUser(job.GetTeacherUser(), $"Поступила оплата за задачу {(int)job.TeacherCost} руб.");
                    break;
                case JobActionType.Reject:
                    NotificateUser(job.GetTeacherUser(), "Задача отправлена Вам на доработку.");
                    break;
                case JobActionType.OfferAbort:
                    NotificateUser(job.GetTeacherUser(), "Заказчик предлагает отменить Вашу задачу.");
                    break;
            }
        }

        private void NotificateUser(ApplicationUser user, string text)
        {
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                smsService.Send(user.PhoneNumber, text + $" {ApplicationSettings.TeacherProjectName}");
            }
        }
    }
}