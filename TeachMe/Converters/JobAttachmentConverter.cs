using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using TeachMe.DataAccess.FileUploading;
using TeachMe.Models;
using TeachMe.Models.Jobs;

namespace TeachMe.Converters
{
    public class JobAttachmentConverter : IJobAttachmentConverter
    {
        private readonly IUploadedFileRepository uploadedFileRepository;
        private const string DefaultMimeType = "application/octet-stream";

        private static readonly Dictionary<string, string> FileExtensionToMimeTypeMap = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                {".gif", "image/gif"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".doc", "application/msword"},
                {".docx", "application/msword"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.ms-excel"},
                {".pdf", "application/pdf"}
            };

        public JobAttachmentConverter(IUploadedFileRepository uploadedFileRepository)
        {
            this.uploadedFileRepository = uploadedFileRepository;
        }

        public JobAttachment FromUploadedJobAttachment(UploadedJobAttachment uploadedJobAttachment)
        {
            return new JobAttachment
            {
                Id = uploadedJobAttachment.Id,
                FileName = uploadedJobAttachment.FileName,
                OriginFileName = uploadedJobAttachment.OriginFileName,
                Type = uploadedJobAttachment.Type
            };
        }

        public FileResult ToFileResult(JobAttachment jobAttachment)
        {
            var fileExtension = Path.GetExtension(jobAttachment.FileName) ?? "";

            string mimeType;
            if (!FileExtensionToMimeTypeMap.TryGetValue(fileExtension, out mimeType))
            {
                mimeType = DefaultMimeType;
            }

            var fileContent = uploadedFileRepository.Read(jobAttachment.FileName);
            return new FileContentResult(fileContent, mimeType) { FileDownloadName = jobAttachment.OriginFileName };
        }
    }
}