
using System;

namespace TeachMe.Models.Jobs
{
    public class JobAttachment
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string OriginFileName { get; set; }
        public JobAttachmentType Type { get; set; }
    }
}