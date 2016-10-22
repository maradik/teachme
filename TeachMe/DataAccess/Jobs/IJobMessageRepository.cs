using System;
using System.Linq.Expressions;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.Jobs
{
    public interface IJobMessageRepository
    {
        JobMessage[] GetAllByJobId(Guid jobId);
        JobMessage[] GetAllByJobIdCreatedAfter(Guid jobId, long ticks);
        void Write(JobMessage model);
        JobMessage Get(Guid id);
        JobMessage[] Get(Expression<Func<JobMessage, bool>> whereExpression = null, Expression<Func<JobMessage, object>> orderByExpression = null, SortOrder sortOrder = SortOrder.Ascending, int? limit = null);
        void Remove(Guid id);
    }
}