using System.Web.Mvc;
using TeachMe.ProjectsSupport;

namespace TeachMe.Controllers
{
    public class ControllerBase : Controller
    {
        private readonly IProjectTypeProvider projectTypeProvider;
        protected ProjectType ProjectType;

        public ControllerBase(IProjectTypeProvider projectTypeProvider)
        {
            this.projectTypeProvider = projectTypeProvider;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ProjectType = projectTypeProvider.Get(filterContext.HttpContext);
            ViewBag.ProjectType = ProjectType;
        }
    }
}