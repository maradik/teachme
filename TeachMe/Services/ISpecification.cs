namespace TeachMe.Services
{
    public interface ISpecification<in TModel>
    {
        bool IsSatisfiedBy(TModel model);
    }
}