using System.Web.Mvc;
using TeachMe.Areas.Student.Models.Shared;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;

namespace TeachMe.Areas.Student.Controllers
{
    [AllowAnonymous]
    public class ArticleController : StudentControllerBase
    {
        private readonly GiftViewModelProvider giftViewModelProvider;

        public ArticleController(IProjectTypeProvider projectTypeProvider,
                                 IProjectInfoProvider projectInfoProvider,
                                 GiftViewModelProvider giftViewModelProvider) 
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.giftViewModelProvider = giftViewModelProvider;
        }

        public ActionResult Referat()
        {
            return View(giftViewModelProvider.Get());
        }

        public ActionResult ReshenieZadach()
        {
            return View(giftViewModelProvider.Get());
        }
    }
}