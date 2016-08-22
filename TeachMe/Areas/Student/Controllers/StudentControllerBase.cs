using System.Net;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;
using TeachMe.ProjectsSupport;
using ControllerBase = TeachMe.Controllers.ControllerBase;

namespace TeachMe.Areas.Student.Controllers
{
    [Authorize]
    public class StudentControllerBase : ControllerBase
    {
        public StudentControllerBase(IProjectTypeProvider projectTypeProvider) : base(projectTypeProvider)
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (ApplicationUser.ProjectType != ProjectType.Student)
            {
                throw new HttpException((int) HttpStatusCode.Forbidden, null);
            }
        }
    }
}