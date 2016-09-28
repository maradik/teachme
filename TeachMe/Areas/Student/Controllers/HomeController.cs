using System.Web.Mvc;
using TeachMe.Areas.Student.Models.Home;
using TeachMe.ProjectsSupport;

namespace TeachMe.Areas.Student.Controllers
{
    [AllowAnonymous]
    public class HomeController : StudentControllerBase
    {
        private IndexRecallViewModelProvider indexRecallViewModelProvider;
        private IndexJobsViewModelProvider indexJobsViewModelProvider;

        public HomeController(IProjectTypeProvider projectTypeProvider,
                              IndexRecallViewModelProvider indexRecallViewModelProvider,
                              IndexJobsViewModelProvider indexJobsViewModelProvider) 
            : base(projectTypeProvider)
        {
            this.indexRecallViewModelProvider = indexRecallViewModelProvider;
            this.indexJobsViewModelProvider = indexJobsViewModelProvider;
        }

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Recalls = indexRecallViewModelProvider.Get(),
                Jobs = indexJobsViewModelProvider.Get()
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