using TeachMe.Models;

namespace TeachMe.Converters
{
    public interface IJobAttachmentConverter
    {
        JobAttachment FromUploadedJobAttachment(UploadedJobAttachment uploadedJobAttachment);
    }
}