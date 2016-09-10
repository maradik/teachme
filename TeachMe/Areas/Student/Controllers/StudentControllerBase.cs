using System.Net;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models.Users;
using TeachMe.ProjectsSupport;
using ControllerBase = TeachMe.Controllers.ControllerBase;

namespace TeachMe.Areas.Student.Controllers
{
    [Authorize(Roles = UserRole.Names.Student)]
    public class StudentControllerBase : ControllerBase
    {
        public StudentControllerBase(IProjectTypeProvider projectTypeProvider) : base(projectTypeProvider)
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (ProjectType != ProjectType.Student)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }
        }
    }
}