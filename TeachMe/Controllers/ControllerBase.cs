using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TeachMe.Models;
using TeachMe.ProjectsSupport;

namespace TeachMe.Controllers
{
    public class ControllerBase : Controller
    {
        private readonly Lazy<ApplicationUser> lazyApplicationUser;
        private readonly Lazy<ProjectType> lazyProjectType;

        public ControllerBase(IProjectTypeProvider projectTypeProvider)
        {
            lazyApplicationUser = new Lazy<ApplicationUser>(() => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId()));
            lazyProjectType = new Lazy<ProjectType>(() => projectTypeProvider.Get(HttpContext));
        }

        protected ApplicationUser ApplicationUser => lazyApplicationUser.Value;
        protected ProjectType ProjectType => lazyProjectType.Value;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ProjectType = ProjectType;
        }
    }
}