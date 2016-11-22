using System.Linq;
using TeachMe.Extensions;
using TeachMe.Helpers.Settings;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.Services.Notifications;

namespace TeachMe.Services.Jobs.JobActionHandlers
{
    public class TeacherNotificationJobCreationHandler : IJobActionCustomHandler
    {
        private readonly ISmsService smsService;
        private readonly ApplicationUserManager applicationUserManager;

        public TeacherNotificationJobCreationHandler(ISmsService smsService, ApplicationUserManager applicationUserManager)
        {
            this.smsService = smsService;
            this.applicationUserManager = applicationUserManager;
        }

        public void Handle(Job job, JobActionType actionType, ApplicationUser user)
        {
            if (actionType != JobActionType.Open)
                return;

            var teacherUsers = applicationUserManager.Users
                                                     .Where(x => x.Roles.Contains(UserRole.Teacher.Name) &&
                                                                 x.SubjectIds.Contains(job.SubjectId) &&
                                                                 x.PhoneNumber != null)
                                                     .OrderByDescending(x => x.CreationTicks)
                                                     .Take(ApplicationSettings.TeachersCountForNewJobNotification)
                                                     .ToArray();

            var jobSubject = job.GetSubject();
            foreach (var teacherUser in teacherUsers)
            {
                NotificateUser(teacherUser, $"Новая задача: {jobSubject.Title}.");
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