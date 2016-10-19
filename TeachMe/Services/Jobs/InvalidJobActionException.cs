using System;
using TeachMe.Models.Jobs;

namespace TeachMe.Services.Jobs
{
    public class InvalidJobActionException : Exception
    {
        public InvalidJobActionException(JobActionType actionType, Guid jobId)
            : base($"Недопустимое действие {actionType} над задачей Id={jobId}")
        {
        }

        public InvalidJobActionException(string message)
            : base(message)
        {
            
        }
    }
}