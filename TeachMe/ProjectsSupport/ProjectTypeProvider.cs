using System.Web;

namespace TeachMe.ProjectsSupport
{
    public class ProjectTypeProvider : IProjectTypeProvider
    {
        public ProjectType Get(HttpContextBase context)
        {
            return context.Request.Url.Port == 8080 ? ProjectType.Teacher : ProjectType.Student;
        }
    }
}