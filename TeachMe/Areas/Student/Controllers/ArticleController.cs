using System.Web.Mvc;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;

namespace TeachMe.Areas.Student.Controllers
{
    [AllowAnonymous]
    public class ArticleController : StudentControllerBase
    {
        public ArticleController(IProjectTypeProvider projectTypeProvider,
                                 IProjectInfoProvider projectInfoProvider) 
            : base(projectTypeProvider, projectInfoProvider)
        {
        }

        public ActionResult Referat()
        {
            return View();
        }

        public ActionResult ReshenieZadach()
        {
            return View();
        }
    }
}