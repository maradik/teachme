using System.Collections.Generic;
using System.Web;
using TeachMe.Models;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess
{
    public interface IUploadedFileRepository
    {
        void Save(UploadedJobAttachment[] uploadedJobAttachments);
        void Save(UploadedJobAttachment uploadedJobAttachment);
        void Delete(string[] fileNames);
        void Delete(string fileName);
    }
}