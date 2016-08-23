using System;
using System.Configuration;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TeachMe.Models;

namespace TeachMe.DataAccess
{
    public class JobRepository : IJobRepository
    {
        public MongoCollection<Job> Collection;

        public JobRepository(JobRepositoryParameters parameters)
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings[parameters.ConnectionStringName].ConnectionString);
            Collection = client.GetServer().GetDatabase(parameters.DatabaseName).GetCollection<Job>(parameters.CollectionName);
        }

        public void Write(Job job)
        {
            if (job.Id == Guid.Empty)
            {
                job.Id = Guid.NewGuid();
                job.CreationTicks = DateTime.UtcNow.Ticks;
            }

            Collection.Save(job);
        }

        public Job Get(Guid id)
        {
            return Collection.FindOneById(id);
        }

        public Job GetByIdAndStudentUserId(Guid id, string studentUserId)
        {
            var job = Get(id);
            return job.StudentUserId == studentUserId
                       ? job
                       : null;
        }

        public Job[] GetAllByStudentUserId(string studentUserId)
        {
            if (studentUserId == null)
                throw new ArgumentNullException(nameof(studentUserId));
            return Collection.Find(Query<Job>.EQ(x => x.StudentUserId, studentUserId)).ToArray();
        }

        public void Remove(Guid id)
        {
            Collection.Remove(Query<Job>.EQ(x => x.Id, id));
        }
    }
}