using System.Web;
using TeachMe.Models;
using TeachMe.Models.Jobs;

namespace TeachMe.Converters
{
    public interface IUploadedFileConverter
    {
        UploadedJobAttachment ToUploadedJobAttachment(HttpPostedFileBase uploadedFile);
    }
}