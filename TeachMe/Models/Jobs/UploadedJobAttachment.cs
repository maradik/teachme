using System.Web;

namespace TeachMe.Models.Jobs
{
    public class UploadedJobAttachment : JobAttachment
    {
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}