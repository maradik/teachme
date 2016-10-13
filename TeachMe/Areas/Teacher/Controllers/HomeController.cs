using System.Web.Mvc;
using TeachMe.Areas.Teacher.Models.Home;
using TeachMe.Models.Users;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;
using IndexViewModel = TeachMe.Areas.Teacher.Models.Home.IndexViewModel;

namespace TeachMe.Areas.Teacher.Controllers
{
    [AllowAnonymous]
    public class HomeController : TeacherControllerBase
    {
        private readonly IndexRecallViewModelProvider indexRecallViewModelProvider;
        private readonly IndexJobsViewModelProvider indexJobsViewModelProvider;
        private readonly IndexUserJobsViewModelProvider indexUserJobsViewModelProvider;

        public HomeController(IProjectTypeProvider projectTypeProvider,
                              IProjectInfoProvider projectInfoProvider,
                              IndexRecallViewModelProvider indexRecallViewModelProvider,
                              IndexJobsViewModelProvider indexJobsViewModelProvider,
                              IndexUserJobsViewModelProvider indexUserJobsViewModelProvider)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.indexRecallViewModelProvider = indexRecallViewModelProvider;
            this.indexJobsViewModelProvider = indexJobsViewModelProvider;
            this.indexUserJobsViewModelProvider = indexUserJobsViewModelProvider;
        }

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Recalls = indexRecallViewModelProvider.Get(),
                LoginViewModel = new LoginViewModel {RememberMe = true},
                Jobs = indexJobsViewModelProvider.Get(),
                UserInfo = new IndexUserInfoViewModel {Jobs = indexUserJobsViewModelProvider.Get(ApplicationUser)}
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