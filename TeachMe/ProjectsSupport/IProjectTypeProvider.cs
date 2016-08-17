using System.Web;

namespace TeachMe.ProjectsSupport
{
    public interface IProjectTypeProvider
    {
        ProjectType Get(HttpContextBase context);
    }
}