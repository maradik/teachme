using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TeachMe.Models;
using TeachMe.Models.Users;
using TeachMe.ProjectsSupport;

namespace TeachMe.Controllers
{
    public class ControllerBase : Controller
    {
        private readonly Lazy<ApplicationUserManager> lazyUserManager;
        private readonly Lazy<ApplicationUser> lazyApplicationUser;
        private readonly Lazy<ProjectType> lazyProjectType;

        public ControllerBase(IProjectTypeProvider projectTypeProvider)
        {
            lazyUserManager = new Lazy<ApplicationUserManager>(() => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            lazyApplicationUser =
                new Lazy<ApplicationUser>(
                    () => !string.IsNullOrEmpty(User.Identity.GetUserId()) ? UserManager.FindById(User.Identity.GetUserId()) : null);
            lazyProjectType = new Lazy<ProjectType>(() => projectTypeProvider.Get(HttpContext));
        }

        protected ApplicationUserManager UserManager => lazyUserManager.Value;
        protected ApplicationUser ApplicationUser => lazyApplicationUser.Value;
        protected ProjectType ProjectType => lazyProjectType.Value;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ProjectType = ProjectType;
            ViewBag.ApplicationUser = ApplicationUser;
        }
    }
}