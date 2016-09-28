using System;
using System.Linq.Expressions;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.Jobs
{
    public interface IJobRepository
    {
        Job Get(Guid id);
        Job GetByIdAndStudentUserId(Guid id, string studentUserId);
        Job GetByIdAndTeacherUserId(Guid id, string teacherUserId);
        Job[] GetAllByStudentUserId(string studentUserId);
        Job[] GetAllByTeacherUserId(string teacherUserId);
        Job[] GetAllByStatus(JobStatus status);
        Job[] Get(Expression<Func<Job, bool>> whereExpression = null, Expression<Func<Job, object>> orderByExpression = null, SortOrder sortOrder = SortOrder.Ascending, int? limit = null);
        void Write(Job job);
        void Remove(Guid id);
    }
}