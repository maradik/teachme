using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models
{
    public class Job
    {
        private List<JobAttachment> attachments;

        public Guid Id { get; set; }

        public int Subject { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        public JobStatus Status { get; set; }

        public decimal Cost { get; set; }

        public List<JobAttachment> Attachments { get { return attachments ?? (attachments = new List<JobAttachment>()); } set { attachments = value; } }

        public string TeacherUserId { get; set; }

        public string StudentUserId { get; set; }
    }
}