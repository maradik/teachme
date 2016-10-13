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
        private readonly IndexUserSuitableJobsViewModelProvider indexUserSuitableJobsViewModelProvider;

        public HomeController(IProjectTypeProvider projectTypeProvider,
                              IProjectInfoProvider projectInfoProvider,
                              IndexRecallViewModelProvider indexRecallViewModelProvider,
                              IndexJobsViewModelProvider indexJobsViewModelProvider,
                              IndexUserJobsViewModelProvider indexUserJobsViewModelProvider,
                              IndexUserSuitableJobsViewModelProvider indexUserSuitableJobsViewModelProvider)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.indexRecallViewModelProvider = indexRecallViewModelProvider;
            this.indexJobsViewModelProvider = indexJobsViewModelProvider;
            this.indexUserJobsViewModelProvider = indexUserJobsViewModelProvider;
            this.indexUserSuitableJobsViewModelProvider = indexUserSuitableJobsViewModelProvider;
        }

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Recalls = indexRecallViewModelProvider.Get(),
                LoginViewModel = new LoginViewModel {RememberMe = true},
                Jobs = indexJobsViewModelProvider.Get()
            };

            if (ApplicationUser != null)
            {
                viewModel.UserInfo.Jobs = indexUserJobsViewModelProvider.Get(ApplicationUser);
                viewModel.UserInfo.SuitableJobs = indexUserSuitableJobsViewModelProvider.Get(ApplicationUser);
                viewModel.UserInfo.Profile = new IndexUserProfileViewModel {UserName = ApplicationUser.UserName, CashMoney = ApplicationUser.Cash.AvailableAmount};
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