using System;
using System.Configuration;
using MongoDB.Driver;
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
            }

            Collection.Save(job);
        }

        public Job Get(Guid id)
        {
            return Collection.FindOneById(id);
        }
    }
}