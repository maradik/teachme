using System;
using System.ComponentModel;
using TeachMe.Models.Users;

namespace TeachMe.Models.Jobs
{
    public class JobMessage : IEntity<Guid>
    {
        private JobAttachment[] attachments;

        public Guid Id { get; set; }
        public Guid JobId { get; set; }

        [DisplayName("Текст")]
        public string Text { get; set; }

        [DisplayName("Фото, документы")]
        public JobAttachment[] Attachments { get { return attachments ?? (attachments = new JobAttachment[0]); } set { attachments = value; } }

        [DisplayName("Автор")]
        public string UserId { get; set; }

        [DisplayName("Дата создания")]
        public long CreationTicks { get; set; }
    }
}