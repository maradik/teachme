using System;
using System.Linq;
using MongoDB.Driver.Builders;
using TeachMe.Models;

namespace TeachMe.DataAccess
{
    public class JobMessageRepository : RepositoryBase<JobMessage>, IJobMessageRepository
    {
        public JobMessageRepository(JobMessageRepositoryParameters parameters) : base(parameters)
        {
        }

        public JobMessage[] GetAllByJobId(Guid jobId)
        {
            return Collection.Find(Query<JobMessage>.EQ(x => x.JobId, jobId)).ToArray();
        }
    }
}