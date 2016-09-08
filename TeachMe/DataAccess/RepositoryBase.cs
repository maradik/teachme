using System;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TeachMe.Models;
using TeachMe.Models.Users;

namespace TeachMe.DataAccess
{
    public class RepositoryBase<TModel> where TModel : IEntity
    {
        protected readonly MongoCollection<TModel> Collection;

        public RepositoryBase(RepositoryBaseParameters parameters)
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings[parameters.ConnectionStringName].ConnectionString);
            Collection = client.GetServer().GetDatabase(parameters.DatabaseName).GetCollection<TModel>(parameters.CollectionName);
        }

        public virtual void Write(TModel model)
        {
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
            }

            if (model.CreationTicks == 0)
            {
                model.CreationTicks = DateTime.UtcNow.Ticks;
            }

            Collection.Save(model);
        }

        public virtual TModel Get(Guid id)
        {
            return Collection.FindOneById(id);
        }

        public virtual void Remove(Guid id)
        {
            Collection.Remove(Query<TModel>.EQ(x => x.Id, id));
        }
    }
}