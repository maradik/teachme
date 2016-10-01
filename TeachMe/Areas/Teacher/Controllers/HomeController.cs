using System.Web.Mvc;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;

namespace TeachMe.Areas.Teacher.Controllers
{
    [AllowAnonymous]
    public class HomeController : TeacherControllerBase
    {
        public HomeController(IProjectTypeProvider projectTypeProvider, IProjectInfoProvider projectInfoProvider) 
            : base(projectTypeProvider, projectInfoProvider)
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