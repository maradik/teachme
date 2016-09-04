using TeachMe.Models.Jobs;
using TeachMe.Models.Users;

namespace TeachMe.Services.Jobs
{
    public interface IJobActionService
    {
        JobActionType[] GetAvailableActions(Job job, ApplicationUser user);
        void DoAction(Job job, JobActionType actionType, ApplicationUser user);
    }
}