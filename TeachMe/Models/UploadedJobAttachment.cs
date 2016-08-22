using System.Web;

namespace TeachMe.Models
{
    public class UploadedJobAttachment : JobAttachment
    {
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}