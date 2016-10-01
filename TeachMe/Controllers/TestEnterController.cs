using System.Web;
using System.Web.Mvc;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;

namespace TeachMe.Controllers
{
    public class TestEnterController : ControllerBase
    {
        public TestEnterController(IProjectTypeProvider projectTypeProvider, IProjectInfoProvider projectInfoProvider) 
            : base(projectTypeProvider, projectInfoProvider)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoToTeacherProject()
        {
            Response.SetCookie(new HttpCookie(ProjectTypeProvider.CookieName, ProjectType.Teacher.ToString()));
            return RedirectToAction("Index", "Home", new {area = ""});
        }

        public ActionResult GoToStudentProject()
        {
            Response.SetCookie(new HttpCookie(ProjectTypeProvider.CookieName, ProjectType.Student.ToString()));
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}