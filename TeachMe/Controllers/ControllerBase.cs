using System.Web.Mvc;
using TeachMe.ProjectsSupport;

namespace TeachMe.Controllers
{
    public class ControllerBase : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ProjectType = new ProjectTypeProvider().Get(filterContext.HttpContext);
        }
    }
}