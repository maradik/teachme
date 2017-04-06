using TeachMe.Models.Jobs;
using TeachMe.Models.Users;

namespace TeachMe.ViewModels.JobChat
{
    public interface IJobMessageViewModelFactory
    {
        JobMessageViewModel Create(JobMessage message, ApplicationUser user);
    }
}