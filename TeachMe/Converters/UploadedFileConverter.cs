using System;
using System.IO;
using System.Web;
using TeachMe.Models;

namespace TeachMe.Converters
{
    public class UploadedFileConverter : IUploadedFileConverter
    {
        public UploadedJobAttachment ToUploadedJobAttachment(HttpPostedFileBase uploadedFile)
        {
            return new UploadedJobAttachment{
                FileName = BuildFileName(uploadedFile),
                OriginFileName = Path.GetFileName(uploadedFile.FileName),
                UploadedFile = uploadedFile
            };
        }

        private string BuildFileName(HttpPostedFileBase uploadedFile)
        {
            return Guid.NewGuid() + Path.GetExtension(uploadedFile.FileName);
        }
    }
}