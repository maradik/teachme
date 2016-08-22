using System.Collections.Generic;
using System.Linq;

namespace TeachMe.References
{
    public class SubjectReference : ISubjectReference
    {
        private static readonly Dictionary<int, Subject> Reference = new[]
        {
            new Subject(1, "Математика"), 
            new Subject(2, "Русский язык"), 
            new Subject(3, "Физика"), 
        }.ToDictionary(x => x.Id);

        public Subject[] GetAll()
        {
            return Reference.Values.ToArray();
        }
    }
}