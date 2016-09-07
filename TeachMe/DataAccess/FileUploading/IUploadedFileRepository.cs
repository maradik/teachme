using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.FileUploading
{
    public interface IUploadedFileRepository
    {
        void Save(UploadedJobAttachment[] uploadedJobAttachments);
        void Save(UploadedJobAttachment uploadedJobAttachment);
        void Delete(string[] fileNames);
        void Delete(string fileName);
    }
}