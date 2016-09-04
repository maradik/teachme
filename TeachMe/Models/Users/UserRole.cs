namespace TeachMe.Models.Users
{
    public class UserRole
    {
        public static readonly UserRole Admin = new UserRole(Names.Admin);
        public static readonly UserRole Teacher = new UserRole(Names.Teacher);
        public static readonly UserRole Student = new UserRole(Names.Student);

        public UserRole(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static class Names
        {
            public const string Admin = "Admin";
            public const string Teacher = "Teacher";
            public const string Student = "Student";
        }
    }
}