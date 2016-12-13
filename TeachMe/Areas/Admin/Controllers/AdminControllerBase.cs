using System.Net;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models.Users;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;
using ControllerBase = TeachMe.Controllers.ControllerBase;

namespace TeachMe.Areas.Admin.Controllers
{
    [Authorize(Roles = UserRole.Names.Admin)]
    public abstract class AdminControllerBase : ControllerBase
    {
        protected AdminControllerBase(IProjectTypeProvider projectTypeProvider, IProjectInfoProvider projectInfoProvider) : 
            base(projectTypeProvider, projectInfoProvider)
        {
        }
    }
}