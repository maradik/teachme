using System.Web.Mvc;
using TeachMe.Areas.Student.Models.Home;
using TeachMe.ProjectsSupport;

namespace TeachMe.Areas.Student.Controllers
{
    [AllowAnonymous]
    public class HomeController : StudentControllerBase
    {
        IndexRecallViewModelProvider indexRecallViewModelProvider;

        public HomeController(IProjectTypeProvider projectTypeProvider,
                              IndexRecallViewModelProvider indexRecallViewModelProvider) 
            : base(projectTypeProvider)
        {
            this.indexRecallViewModelProvider = indexRecallViewModelProvider;
        }

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Recalls = indexRecallViewModelProvider.Get()
            };
            return View(viewModel);
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