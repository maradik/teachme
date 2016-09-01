using System;
using System.Linq;
using System.Net;
using System.Web;
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

        public ActionResult IndexAvailable()
        {
            var jobs = jobRepository.GetAllByStatus(JobStatus.Opened).OrderByDescending(x => x.CreationTicks).ToArray();
            return View(jobs);
        }

        public ActionResult Details(Guid id)
        {
            var job = jobRepository.Get(id);

            if (!string.IsNullOrEmpty(job.TeacherUserId) && job.TeacherUserId != ApplicationUser.Id)
                throw new HttpException((int) HttpStatusCode.Forbidden, null);

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Take(Guid id)
        {
            var job = jobRepository.GetByIdAndTeacherUserId(id, null);

            if (job.Status != JobStatus.Opened)
                throw new InvalidOperationException($"Невозможно закрепить за исполнителем {ApplicationUser.UserName} задачу {job.Id}, т.к. у нее статус {job.Status}");

            job.TeacherUserId = ApplicationUser.Id;
            job.Status = JobStatus.InWorking;

            jobRepository.Write(job);

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoJobAction(Guid jobId, JobActionType jobActionType)
        {
            var job = jobRepository.GetByIdAndTeacherUserId(jobId, ApplicationUser.Id);
            job.DoAction(jobActionType, ApplicationUser.ProjectType);
            jobRepository.Write(job);
            return RedirectToAction(nameof(Details), new { id = jobId });
        }
    }
}