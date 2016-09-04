using TeachMe.Models;
using TeachMe.Models.Jobs;

namespace TeachMe.Converters
{
    public class JobAttachmentConverter : IJobAttachmentConverter
    {
        public JobAttachment FromUploadedJobAttachment(UploadedJobAttachment uploadedJobAttachment)
        {
            return new JobAttachment
            {
                FileName = uploadedJobAttachment.FileName,
                OriginFileName = uploadedJobAttachment.OriginFileName,
                Type = uploadedJobAttachment.Type
            };
        }
    }
}