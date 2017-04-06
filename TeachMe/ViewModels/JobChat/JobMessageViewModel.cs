using System;
using System.ComponentModel;
using TeachMe.Models;

namespace TeachMe.ViewModels.JobChat
{
    public class JobMessageViewModel : IWithUserId
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }

        [DisplayName("Текст")]
        public string Text { get; set; }

        [DisplayName("Фото, документы")]
        public IJobMessageAttachmentViewModel[] Attachments { get; set; }

        [DisplayName("Автор")]
        public string UserId { get; set; }

        [DisplayName("Дата создания")]
        public long CreationTicks { get; set; }
    }
}