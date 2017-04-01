using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TeachMe.Helpers.Settings;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.FileUploading
{
    public class UploadedFileRepository : IUploadedFileRepository
    {
        private static readonly HashSet<string> AllowedUploadFileExtensions;

        static UploadedFileRepository()
        {
            AllowedUploadFileExtensions = new HashSet<string>(ApplicationSettings.AllowedUploadFileExtensions, StringComparer.InvariantCultureIgnoreCase);
        }

        public byte[] Read(string fileName)
        {
            return File.ReadAllBytes(BuildFullName(fileName));
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