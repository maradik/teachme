using System;

namespace TeachMe.Models
{
    public class JobMessage : IEntity
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string Text { get; set; }
        public JobAttachment[] Attachments { get; set; }
        public string UserId { get; set; }
        public long CreationTicks { get; set; }
    }
}