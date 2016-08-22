using System.Web;
using TeachMe.Models;

namespace TeachMe.Converters
{
    public interface IUploadedFileConverter
    {
        UploadedJobAttachment ToUploadedJobAttachment(HttpPostedFileBase uploadedFile);
    }
}