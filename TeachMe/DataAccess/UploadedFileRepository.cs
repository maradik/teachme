using System.Collections.Generic;
using System.IO;
using System.Web;
using TeachMe.Models;

namespace TeachMe.DataAccess
{
    public class UploadedFileRepository : IUploadedFileRepository
    {
        public void Save(IEnumerable<UploadedJobAttachment> uploadedJobAttachments)
        {
            foreach (var uploadedJobAttachment in uploadedJobAttachments)
            {
                Save(uploadedJobAttachment);
            }
        }

        public void Save(UploadedJobAttachment uploadedJobAttachment)
        {
            uploadedJobAttachment.UploadedFile.SaveAs(BuildFullName(uploadedJobAttachment));
        }

        private string BuildFullName(UploadedJobAttachment uploadedJobAttachment)
        {
            return HttpContext.Current.Server.MapPath(VirtualPathUtility.Combine("~/Uploads/", uploadedJobAttachment.FileName));
        }
    }
}