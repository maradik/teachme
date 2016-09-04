using System;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;

namespace TeachMe.Services.Jobs
{
    public interface IJobActionService
    {
        JobActionType[] GetAvailableActions(Job job, ApplicationUser user);
        Job DoAction(Guid jobId, JobActionType actionType, ApplicationUser user);
    }
}