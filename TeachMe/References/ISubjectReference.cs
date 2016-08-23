namespace TeachMe.References
{
    public interface ISubjectReference
    {
        Subject Get(int id);
        Subject[] GetAll();
    }
}