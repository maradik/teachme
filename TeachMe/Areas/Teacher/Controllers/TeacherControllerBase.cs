using System.Web.Mvc;
using TeachMe.Models.Users;
using TeachMe.ProjectsSupport;
using ControllerBase = TeachMe.Controllers.ControllerBase;

namespace TeachMe.Areas.Teacher.Controllers
{
    [Authorize(Roles = UserRole.Names.Teacher)]
    public class TeacherControllerBase : ControllerBase
    {
        public TeacherControllerBase(IProjectTypeProvider projectTypeProvider) : base(projectTypeProvider)
        {
        }
    }
}