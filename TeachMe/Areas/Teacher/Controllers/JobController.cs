using System.Linq;
using System.Web.Mvc;
using TeachMe.DataAccess;
using TeachMe.Models;
using TeachMe.ProjectsSupport;

namespace TeachMe.Areas.Teacher.Controllers
{
    public class JobController : TeacherControllerBase
    {
        private readonly IJobRepository jobRepository;

        public JobController(IProjectTypeProvider projectTypeProvider,
                             IJobRepository jobRepository)
            : base(projectTypeProvider)
        {
            this.jobRepository = jobRepository;
        }

        public ActionResult Index()
        {
            var jobs = jobRepository.GetAllByTeacherUserId(ApplicationUser.Id).OrderByDescending(x => x.CreationTicks).ToArray();
            return View(jobs);
        }
    }
}