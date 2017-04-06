using TeachMe.Models.Jobs;
using TeachMe.Models.Users;

namespace TeachMe.Services.JobChat
{
    public interface IJobMessageAttachmentAccessService
    {
        bool HasAccess(JobMessage message, ApplicationUser user);
    }
}