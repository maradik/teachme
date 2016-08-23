using System;
using TeachMe.Models;

namespace TeachMe.DataAccess
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