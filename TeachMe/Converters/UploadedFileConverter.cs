using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using TeachMe.Models;
using TeachMe.Models.Jobs;

namespace TeachMe.Converters
{
    public class UploadedFileConverter : IUploadedFileConverter
    {
        private static readonly Dictionary<string, JobAttachmentType> FileExtensionToTypeMap =
            new Dictionary<string, JobAttachmentType>(StringComparer.InvariantCultureIgnoreCase)
            {
                {".gif", JobAttachmentType.Image},
                {".png", JobAttachmentType.Image},
                {".jpg", JobAttachmentType.Image},
                {".jpeg", JobAttachmentType.Image},
            };

        public UploadedJobAttachment ToUploadedJobAttachment(HttpPostedFileBase uploadedFile)
        {
            return new UploadedJobAttachment
            {
                FileName = BuildFileName(uploadedFile),
                OriginFileName = Path.GetFileName(uploadedFile.FileName),
                Type = GetAttachmentFileType(uploadedFile),
                UploadedFile = uploadedFile
            };
        }

        private JobAttachmentType GetAttachmentFileType(HttpPostedFileBase uploadedFile)
        {
            var fileExtension = Path.GetExtension(uploadedFile.FileName);
            return FileExtensionToTypeMap.ContainsKey(fileExtension)
                       ? FileExtensionToTypeMap[fileExtension]
                       : JobAttachmentType.Undefined;
        }

        private string BuildFileName(HttpPostedFileBase uploadedFile)
        {
            return Guid.NewGuid() + Path.GetExtension(uploadedFile.FileName);
        }
    }
}