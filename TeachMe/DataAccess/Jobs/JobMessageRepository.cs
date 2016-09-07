using System;
using System.Linq;
using MongoDB.Driver.Builders;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.Jobs
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

        public JobMessage[] GetAllByJobIdCreatedAfter(Guid jobId, long ticks)
        {
            var query = Query.And(Query<JobMessage>.EQ(x => x.JobId, jobId),
                                  Query<JobMessage>.GT(x => x.CreationTicks, ticks));
            return Collection.Find(query).ToArray();
        }
    }
}