using System;
using System.Linq;
using MongoDB.Driver.Builders;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.Jobs
{
    public class JobRepository : RepositoryBase<Job>, IJobRepository
    {
        public JobRepository(JobRepositoryParameters parameters) : base(parameters)
        {
        }

        public Job GetByIdAndStudentUserId(Guid id, string studentUserId)
        {
            var job = Get(id);
            return job.StudentUserId == studentUserId || (string.IsNullOrEmpty(job.StudentUserId) && string.IsNullOrEmpty(studentUserId))
                       ? job
                       : null;
        }

        public Job GetByIdAndTeacherUserId(Guid id, string teacherUserId)
        {
            var job = Get(id);
            return job.TeacherUserId == teacherUserId || (string.IsNullOrEmpty(job.TeacherUserId) && string.IsNullOrEmpty(teacherUserId))
                       ? job
                       : null;
        }

        public Job[] GetAllByStudentUserId(string studentUserId)
        {
            if (studentUserId == null)
                throw new ArgumentNullException(nameof(studentUserId));
            return Collection.Find(Query<Job>.EQ(x => x.StudentUserId, studentUserId)).ToArray();
        }

        public Job[] GetAllByTeacherUserId(string teacherUserId)
        {
            if (teacherUserId == null)
                throw new ArgumentNullException(nameof(teacherUserId));
            return Collection.Find(Query<Job>.EQ(x => x.TeacherUserId, teacherUserId)).ToArray();
        }

        public Job[] GetAllByStatus(JobStatus status)
        {
            return Collection.Find(Query<Job>.EQ(x => x.Status, status)).ToArray();
        }
    }
}