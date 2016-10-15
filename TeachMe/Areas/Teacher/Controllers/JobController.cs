using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.Extensions;
using TeachMe.Models.Jobs;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;
using TeachMe.Services.Jobs;
using TeachMe.ViewModels.Jobs;

namespace TeachMe.Areas.Teacher.Controllers
{
    public class JobController : TeacherControllerBase
    {
        private readonly IJobActionService jobActionService;
        private readonly IJobRepository jobRepository;

        public JobController(IProjectTypeProvider projectTypeProvider,
                             IProjectInfoProvider projectInfoProvider,
                             IJobActionService jobActionService,
                             IJobRepository jobRepository)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.jobActionService = jobActionService;
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

            return View(new JobDetailsViewModel {Job = job, JobAvailableActions = jobActionService.GetAvailableActions(job, ApplicationUser)});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoJobAction(Guid jobId, JobActionType jobActionType, string redirectActionName = nameof(Details))
        {
            jobActionService.DoAction(jobId, jobActionType, ApplicationUser);
            return RedirectToAction(redirectActionName, new {id = jobId});
        }

        public ActionResult _GetAvailableActions(Guid jobId)
        {
            var job = jobRepository.Get(jobId);
            var availableActions = jobActionService.GetAvailableActions(job, ApplicationUser);
            return Json(availableActions.Select(x => new { Value = (int)x, Text = x.GetHumanAnnotation() }), JsonRequestBehavior.AllowGet);
        }
    }
}