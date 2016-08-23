using System;
using TeachMe.References;

namespace TeachMe.Extensions
{
    public static class JobExtensions
    {
        public static Subject GetSubject(this Models.Job job)
        {
            return SubjectReference.Instance.Get(job.SubjectId);
        }

        public static DateTime GetCreationDateTime(this Models.Job job)
        {
            return new DateTime(job.CreationTicks, DateTimeKind.Utc);
        }
    }
}