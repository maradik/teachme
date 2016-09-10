using System.Web.Mvc;
using TeachMe.ProjectsSupport;

namespace TeachMe.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IProjectTypeProvider projectTypeProvider)
            : base(projectTypeProvider)
        {
        }

        public ActionResult Index()
        {
            return ProjectType == ProjectType.Student
                       ? RedirectToAction("Index", "Home", new {area = "Student"})
                       : RedirectToAction("Index", "Home", new {area = "Teacher"});
        }
    }
}