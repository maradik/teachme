using System.Web.Mvc;
using TeachMe.ProjectsSupport;

namespace TeachMe.Areas.Teacher.Controllers
{
    [AllowAnonymous]
    public class HomeController : TeacherControllerBase
    {
        public HomeController(IProjectTypeProvider projectTypeProvider) : base(projectTypeProvider)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}