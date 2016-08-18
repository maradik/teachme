namespace TeachMe.DataAccess
{
    public class JobRepositoryParameters
    {
        public string ConnectionStringName => "Mongo";

        public string DatabaseName => "TeachMe";

        public string CollectionName => "Jobs";
    }
}