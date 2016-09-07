using System;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.FileUploading
{
    public class NotAllowedUploadedJobAttachmentException : Exception
    {
        private const string DefaultMessage = "Файлы-приложения к задаче запрещены";

        public NotAllowedUploadedJobAttachmentException(UploadedJobAttachment uploadedJobAttachment, string message = DefaultMessage) 
            : this(new[] { uploadedJobAttachment }, message)
        {
        }

        public NotAllowedUploadedJobAttachmentException(UploadedJobAttachment[] uploadedJobAttachments, string message = DefaultMessage) 
            : base(message)
        {
            UploadedJobAttachments = uploadedJobAttachments;
        }

        public UploadedJobAttachment[] UploadedJobAttachments { get; }
    }
}