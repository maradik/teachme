using System.Web.Mvc;
using TeachMe.Areas.Teacher.Models.Home;
using TeachMe.Helpers.Settings;
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
            if (ApplicationUser != null)
            {
                var viewModel = new IndexUserInfoViewModel
                {
                    Jobs = indexUserJobsViewModelProvider.Get(ApplicationUser),
                    SuitableJobs = indexUserSuitableJobsViewModelProvider.Get(ApplicationUser),
                    Profile = new IndexUserProfileViewModel {UserName = ApplicationUser.UserName, CashMoney = ApplicationUser.Cash.AvailableAmount}
                };
                return View("UserIndex", viewModel);
            }
            else
            {
                var viewModel = new IndexViewModel
                {
                    Recalls = indexRecallViewModelProvider.Get(),
                    LoginViewModel = new LoginViewModel {RememberMe = true},
                    Jobs = indexJobsViewModelProvider.Get()
                };
                return View("Index", viewModel);
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            var contactViewModel = new ContactViewModel { Email = ApplicationSettings.TeacherContactEmail, Phone = ApplicationSettings.TeacherContactPhone };
            return View(contactViewModel);
        }
    }
}