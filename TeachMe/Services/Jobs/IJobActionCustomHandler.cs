using TeachMe.Models.Jobs;
using TeachMe.Models.Users;

namespace TeachMe.Services.Jobs
{
    public interface IJobActionCustomHandler
    {
        void Handle(Job job, JobActionType actionType, ApplicationUser user);
    }
}