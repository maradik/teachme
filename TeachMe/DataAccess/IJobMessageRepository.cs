using System;
using TeachMe.Models;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess
{
    public interface IJobMessageRepository
    {
        JobMessage[] GetAllByJobId(Guid jobId);
        JobMessage[] GetAllByJobIdCreatedAfter(Guid jobId, long ticks);
        void Write(JobMessage model);
        JobMessage Get(Guid id);
        void Remove(Guid id);
    }
}