using System;
using System.Web;

namespace TeachMe.ProjectsSupport
{
    public class ProjectTypeProvider : IProjectTypeProvider
    {
        public const string CookieName = "TeachMeProjectType";

        public ProjectType Get(HttpContextBase context)
        {
#if DEBUG
            var projectTypeCookie = context.Request.Cookies[CookieName];
            ProjectType projectTypeFromCookie;
            if (projectTypeCookie != null && Enum.TryParse(projectTypeCookie.Value, out projectTypeFromCookie))
            {
                return projectTypeFromCookie;
            }
#endif
            return context.Request.Url.Port == 8080 ? ProjectType.Teacher : ProjectType.Student;
        }
    }
}