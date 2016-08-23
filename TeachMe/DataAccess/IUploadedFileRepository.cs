using System.Collections.Generic;
using System.Web;
using TeachMe.Models;

namespace TeachMe.DataAccess
{
    public interface IUploadedFileRepository
    {
        void Save(UploadedJobAttachment[] uploadedJobAttachments);
        void Save(UploadedJobAttachment uploadedJobAttachment);
    }
}