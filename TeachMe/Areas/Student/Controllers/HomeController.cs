using System.Web.Mvc;
using TeachMe.Areas.Student.Models.Home;
using TeachMe.Helpers.Settings;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;

namespace TeachMe.Areas.Student.Controllers
{
    [AllowAnonymous]
    public class HomeController : StudentControllerBase
    {
        private readonly IndexRecallViewModelProvider indexRecallViewModelProvider;
        private readonly IndexJobsViewModelProvider indexJobsViewModelProvider;
        private readonly IndexUserJobsViewModelProvider indexUserJobsViewModelProvider;
        private readonly IndexUserJobMessagesViewModelProvider indexUserJobMessagesViewModelProvider;

        public HomeController(IProjectTypeProvider projectTypeProvider,
                              IProjectInfoProvider projectInfoProvider,
                              IndexRecallViewModelProvider indexRecallViewModelProvider,
                              IndexJobsViewModelProvider indexJobsViewModelProvider,
                              IndexUserJobsViewModelProvider indexUserJobsViewModelProvider,
                              IndexUserJobMessagesViewModelProvider indexUserJobMessagesViewModelProvider) 
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.indexRecallViewModelProvider = indexRecallViewModelProvider;
            this.indexJobsViewModelProvider = indexJobsViewModelProvider;
            this.indexUserJobsViewModelProvider = indexUserJobsViewModelProvider;
            this.indexUserJobMessagesViewModelProvider = indexUserJobMessagesViewModelProvider;
        }

        public ActionResult Index()
        {
            ViewBag.ReturnUrl = Url.Action("Index", "Home", new { area = "" });

            if (ApplicationUser != null)
            {
                var viewModel = new IndexUserInfoViewModel
                {
                    Jobs = indexUserJobsViewModelProvider.Get(ApplicationUser),
                    JobMessages = indexUserJobMessagesViewModelProvider.Get(ApplicationUser),
                    Profile = new IndexUserProfileViewModel { UserName = ApplicationUser.UserName, CashMoney = ApplicationUser.Cash.AvailableAmount }
                };
                return View("UserIndex", viewModel);
            }
            else
            {
                var viewModel = new IndexViewModel
                {
                    Recalls = indexRecallViewModelProvider.Get(),
                    Jobs = indexJobsViewModelProvider.Get(),
                    LoginViewModel = new TeachMe.Models.Users.LoginViewModel { RememberMe = true },
                    GiftAmountForNewUser = (int) ApplicationSettings.StudentInitialCash
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
            var contactViewModel = new ContactViewModel { Email = ApplicationSettings.StudentContactEmail };
            return View(contactViewModel);
        }
    }
}