using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using TeachMe.Models;

namespace TeachMe.DataAccess
{
    public class UploadedFileRepository : IUploadedFileRepository
    {
        private static readonly HashSet<string> AllowedUploadFileExtensions;

        static UploadedFileRepository()
        {
            var allowedUploadFileExtensionsFromSettings = ConfigurationManager.AppSettings["AllowedUploadFileExtensions"]?.Split(',');
            AllowedUploadFileExtensions = new HashSet<string>(allowedUploadFileExtensionsFromSettings ?? new string[0], StringComparer.InvariantCultureIgnoreCase);
        }

        public void Save(UploadedJobAttachment[] uploadedJobAttachments)
        {
            var notAllowedUploadedJobAttachments = uploadedJobAttachments.Where(x => !IsUploadedJobAttachmentAllowed(x)).ToArray();
            if (notAllowedUploadedJobAttachments.Length > 0)
                throw new NotAllowedUploadedJobAttachmentException(notAllowedUploadedJobAttachments);

            foreach (var uploadedJobAttachment in uploadedJobAttachments)
            {
                Save(uploadedJobAttachment);
            }
        }

        public void Save(UploadedJobAttachment uploadedJobAttachment)
        {
            if (!IsUploadedJobAttachmentAllowed(uploadedJobAttachment))
            {
                throw new NotAllowedUploadedJobAttachmentException(uploadedJobAttachment);
            }

            uploadedJobAttachment.UploadedFile.SaveAs(BuildFullName(uploadedJobAttachment.FileName));
        }

        public void Delete(string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                Delete(fileName);
            }
        }

        public void Delete(string fileName)
        {
            File.Delete(BuildFullName(fileName));
        }

        private static bool IsUploadedJobAttachmentAllowed(UploadedJobAttachment uploadedJobAttachment)
        {
            var fileNameExtension = Path.GetExtension(uploadedJobAttachment.FileName);
            return AllowedUploadFileExtensions.Contains(fileNameExtension);
        }

        private string BuildFullName(string fileName)
        {
            return HttpContext.Current.Server.MapPath(VirtualPathUtility.Combine("~/Uploads/", fileName));
        }
    }
}