namespace TeachMe.DataAccess
{
    public abstract class RepositoryBaseParameters
    {
        public virtual string ConnectionStringName => "Mongo";

        public virtual string DatabaseName => "TeachMe";

        public abstract string CollectionName { get; }
    }
}