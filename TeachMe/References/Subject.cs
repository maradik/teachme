namespace TeachMe.References
{
    public class Subject
    {
        public Subject(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; private set; }

        public string Title { get; private set; }
    }
}