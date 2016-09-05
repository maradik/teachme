using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.References;

namespace TeachMe.Extensions
{
    public static class JobExtensions
    {
        public static Subject GetSubject(this Job job)
        {
            return SubjectReference.Instance.Get(job.SubjectId);
        }

        public static DateTime GetCreationDateTime(this Job job)
        {
            return new DateTime(job.CreationTicks, DateTimeKind.Utc);
        }

        public static ApplicationUser GetStudentUser(this Job job)
        {
            return string.IsNullOrEmpty(job.StudentUserId)
                       ? null
                       : HttpContext.Current.GetOwinContext()
                                    .GetUserManager<ApplicationUserManager>()
                                    .FindById(job.StudentUserId);
        }

        public static ApplicationUser GetTeacherUser(this Job job)
        {
            return string.IsNullOrEmpty(job.TeacherUserId)
                       ? null
                       : HttpContext.Current.GetOwinContext()
                                    .GetUserManager<ApplicationUserManager>()
                                    .FindById(job.TeacherUserId);
        }
    }
}