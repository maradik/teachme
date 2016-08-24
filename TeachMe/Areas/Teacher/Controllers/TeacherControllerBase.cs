using System.Net;
using System.Web;
using System.Web.Mvc;
using TeachMe.ProjectsSupport;
using ControllerBase = TeachMe.Controllers.ControllerBase;

namespace TeachMe.Areas.Teacher.Controllers
{
    [Authorize]
    public class TeacherControllerBase : ControllerBase
    {
        public TeacherControllerBase(IProjectTypeProvider projectTypeProvider) : base(projectTypeProvider)
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (ApplicationUser.ProjectType != ProjectType.Teacher)
            {
                throw new HttpException((int) HttpStatusCode.Forbidden, null);
            }
        }
    }
}