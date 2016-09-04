using System;
using TeachMe.Models.Users;

namespace TeachMe.Models.Jobs
{
    public class JobMessage : IEntity
    {
        private JobAttachment[] attachments;

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string Text { get; set; }
        public JobAttachment[] Attachments { get { return attachments ?? (attachments = new JobAttachment[0]); } set { attachments = value; } }
        public string UserId { get; set; }
        public long CreationTicks { get; set; }
    }
}