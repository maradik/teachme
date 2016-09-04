using System.Net;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;
using TeachMe.Models.Users;
using TeachMe.ProjectsSupport;
using ControllerBase = TeachMe.Controllers.ControllerBase;

namespace TeachMe.Areas.Student.Controllers
{
    [Authorize(Roles = UserRole.Names.Student)]
    public class StudentControllerBase : ControllerBase
    {
        public StudentControllerBase(IProjectTypeProvider projectTypeProvider) : base(projectTypeProvider)
        {
        }
    }
}