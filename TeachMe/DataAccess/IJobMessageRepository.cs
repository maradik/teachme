using System;
using TeachMe.Models;

namespace TeachMe.DataAccess
{
    public interface IJobMessageRepository
    {
        JobMessage[] GetAllByJobId(Guid jobId);
        void Write(JobMessage model);
        JobMessage Get(Guid id);
        void Remove(Guid id);
    }
}