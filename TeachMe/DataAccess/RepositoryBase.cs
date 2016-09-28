using System;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TeachMe.Models;
using System.Linq.Expressions;
using System.Linq;

namespace TeachMe.DataAccess
{
    public abstract class RepositoryBase<TModel, TModelId> 
        where TModel : IEntity<TModelId> 
        where TModelId : struct, IComparable
    {
        protected readonly MongoCollection<TModel> Collection;

        protected RepositoryBase(RepositoryBaseParameters parameters)
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings[parameters.ConnectionStringName].ConnectionString);
            Collection = client.GetServer().GetDatabase(parameters.DatabaseName).GetCollection<TModel>(parameters.CollectionName);
        }

        public virtual void Write(TModel model)
        {
            if (model.Id.CompareTo(default(TModelId)) == 0)
            {
                model.Id = CreateNewId();
            }

            if (model.CreationTicks == 0)
            {
                model.CreationTicks = DateTime.UtcNow.Ticks;
            }

            Collection.Save(model);
        }

        public virtual TModel Get(TModelId id)
        {
            return Collection.FindOne(Query<TModel>.EQ(x => x.Id, id));
        }

        public virtual void Remove(TModelId id)
        {
            Collection.Remove(Query<TModel>.EQ(x => x.Id, id));
        }

        public TModel[] Get(Expression<Func<TModel, bool>> whereExpression = null, Expression<Func<TModel, object>> orderByExpression = null, SortOrder sortOrder = SortOrder.Ascending, int? limit = null)
        {
            var query = whereExpression != null ? Query<TModel>.Where(whereExpression) : Query.Null;
            var result = Collection.Find(query);

            if (orderByExpression != null)
            {
                var sortBy = sortOrder == SortOrder.Ascending ? SortBy<TModel>.Ascending(orderByExpression) : SortBy<TModel>.Descending(orderByExpression);
                result = result.SetSortOrder(sortBy);
            }

            if (limit != null)
            {
                result = result.SetLimit(limit.Value);
            }

            return result.ToArray();
        }

        protected abstract TModelId CreateNewId();
    }
}