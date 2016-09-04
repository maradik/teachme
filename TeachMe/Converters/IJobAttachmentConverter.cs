using TeachMe.Models;
using TeachMe.Models.Jobs;

namespace TeachMe.Converters
{
    public interface IJobAttachmentConverter
    {
        JobAttachment FromUploadedJobAttachment(UploadedJobAttachment uploadedJobAttachment);
    }
}