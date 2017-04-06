using System;
using System.ComponentModel;
using System.Linq;
using TeachMe.Models;
using TeachMe.Models.Jobs;

namespace TeachMe.ViewModels.JobChat
{
    public class JobMessageViewModel : IWithUserId
    {
        private JobMessageAttachmentViewModel[] attachments;

        public JobMessageViewModel(JobMessage entity)
        {
            Id = entity.Id;
            JobId = entity.JobId;
            Text = entity.Text;
            UserId = entity.UserId;
            Attachments = entity.Attachments.Select(x => new JobMessageAttachmentViewModel(x, Id)).ToArray();
            CreationTicks = entity.CreationTicks;
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }

        [DisplayName("Текст")]
        public string Text { get; set; }

        [DisplayName("Фото, документы")]
        public JobMessageAttachmentViewModel[] Attachments { get { return attachments ?? (attachments = new JobMessageAttachmentViewModel[0]); } set { attachments = value; } }

        [DisplayName("Автор")]
        public string UserId { get; set; }

        [DisplayName("Дата создания")]
        public long CreationTicks { get; set; }
    }
}