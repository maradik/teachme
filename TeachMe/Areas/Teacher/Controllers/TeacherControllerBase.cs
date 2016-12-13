using System.Net;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models.Users;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;
using ControllerBase = TeachMe.Controllers.ControllerBase;

namespace TeachMe.Areas.Teacher.Controllers
{
    [Authorize(Roles = UserRole.Names.Teacher)]
    public abstract class TeacherControllerBase : ControllerBase
    {
        protected TeacherControllerBase(IProjectTypeProvider projectTypeProvider, IProjectInfoProvider projectInfoProvider) 
            : base(projectTypeProvider, projectInfoProvider)
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (ProjectType != ProjectType.Teacher)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }
        }
    }
}