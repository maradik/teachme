using System.Collections.Generic;
using System.Linq;

namespace TeachMe.References
{
    public class SubjectReference : ISubjectReference
    {
        private static readonly Dictionary<int, Subject> Reference = new[]
        {
            new Subject(1, "Математика 5-6 класс"),
            new Subject(2, "Алгебра 7-9 класс"),
            new Subject(3, "Алгебра 10-11 класс"),
            new Subject(4, "Геометрия 7-9 класс"),
            new Subject(5, "Геометрия 10-11 класс"),
            new Subject(6, "Русский язык 5-6 класс"),
            new Subject(7, "Русский язык 7-9 класс"),
            new Subject(8, "Русский язык 10-11 класс"),
            new Subject(9, "Физика 7-9 класс"),
            new Subject(10, "Физика 10-11 класс"),
            new Subject(11, "Химия 8-9 класс"),
            new Subject(12, "Химия 10-11 класс"),
            new Subject(13, "Иностранный язык 5-9 класс"),
            new Subject(14, "Иностранный язык 10-11 класс"),
            new Subject(101, "Дипломная работа (студент)"),
            new Subject(102, "Курсовая работа (студент)"),
            new Subject(103, "Контрольная работа (студент)"),
            new Subject(104, "Реферат (студент)"),
            new Subject(105, "Доклад (студент)"),

        }.ToDictionary(x => x.Id);

        private static ISubjectReference instance;

        public static ISubjectReference Instance => instance ?? (instance = new SubjectReference());

        public Subject Get(int id)
        {
            return Reference.ContainsKey(id)
                       ? Reference[id]
                       : null;
        }

        public Subject[] GetAll()
        {
            return Reference.Values.ToArray();
        }
    }
}