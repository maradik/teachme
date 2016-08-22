using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models
{
    public class Job
    {
        private List<JobAttachment> attachments;

        public Guid Id { get; set; }

        [Display(Name = "Предмет")]
        [Range(1, int.MaxValue, ErrorMessage = "Выберите значение из списка")]
        public int SubjectId { get; set; }

        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Статус")]
        public JobStatus Status { get; set; }

        [Display(Name = "Стоимость")]
        public decimal Cost { get; set; }

        [DisplayName("Фото, документы")]
        public List<JobAttachment> Attachments { get { return attachments ?? (attachments = new List<JobAttachment>()); } set { attachments = value; } }

        public string TeacherUserId { get; set; }

        public string StudentUserId { get; set; }

        public long CreationTicks { get; set; }
    }
}