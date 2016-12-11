using System;
using System.IO;
using System.Linq;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.FileUploading
{
    public class NotAllowedUploadedJobAttachmentException : Exception
    {
        private const string DefaultMessage = "Файлы-приложения к задаче запрещены.";

        public NotAllowedUploadedJobAttachmentException(UploadedJobAttachment uploadedJobAttachment, string message = DefaultMessage) 
            : this(new[] { uploadedJobAttachment }, message)
        {
        }

        public NotAllowedUploadedJobAttachmentException(UploadedJobAttachment[] uploadedJobAttachments, string message = DefaultMessage) 
            : base(BuildFullMessage(uploadedJobAttachments, message))
        {
            UploadedJobAttachments = uploadedJobAttachments;
        }

        public UploadedJobAttachment[] UploadedJobAttachments { get; }

        private static string BuildFullMessage(UploadedJobAttachment[] uploadedJobAttachments, string message)
        {
            return message + " Расширения:" + string.Join(",", uploadedJobAttachments.Select(x => Path.GetExtension(x.FileName)));
        }
    }
}