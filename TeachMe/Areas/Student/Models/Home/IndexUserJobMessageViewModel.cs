using System;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexUserJobMessageViewModel
    {
        public Guid JobId { get; set; }
        public string JobTitle { get; set; }
        public string Text { get; set; }
        public bool HasAttachments { get; set; }
        public string AuthorName { get; set; }
        public long CreationTicks { get; set; }
    }
}