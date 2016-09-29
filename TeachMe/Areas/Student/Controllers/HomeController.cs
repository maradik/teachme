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
        private IndexUserJobsViewModelProvider indexUserJobsViewModelProvider;

        public HomeController(IProjectTypeProvider projectTypeProvider,
                              IndexRecallViewModelProvider indexRecallViewModelProvider,
                              IndexJobsViewModelProvider indexJobsViewModelProvider,
                              IndexUserJobsViewModelProvider indexUserJobsViewModelProvider) 
            : base(projectTypeProvider)
        {
            this.indexRecallViewModelProvider = indexRecallViewModelProvider;
            this.indexJobsViewModelProvider = indexJobsViewModelProvider;
            this.indexUserJobsViewModelProvider = indexUserJobsViewModelProvider;
        }

        public ActionResult Index()
        {
            ViewBag.ReturnUrl = Url.Action("Index", "Home", new { area = "" });
            var viewModel = new IndexViewModel
            {
                Recalls = indexRecallViewModelProvider.Get(),
                Jobs = indexJobsViewModelProvider.Get()
            };

            if (ApplicationUser != null)
            {
                viewModel.UserInfo.Jobs = indexUserJobsViewModelProvider.Get(ApplicationUser);
            }

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