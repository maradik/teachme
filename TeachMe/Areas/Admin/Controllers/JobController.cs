using System;
using System.Web.Mvc;
using log4net;
using TeachMe.Areas.Admin.Models.Jobs;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.ProjectsSupport;
using TeachMe.References;
using TeachMe.Services.General;

namespace TeachMe.Areas.Admin.Controllers
{
    public class JobController : AdminControllerBase
    {
        private readonly IJobRepository jobRepository;

        public JobController(IJobRepository jobRepository,
                             ISubjectReference subjectReference,
                             IProjectTypeProvider projectTypeProvider,
                             IProjectInfoProvider projectInfoProvider)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.jobRepository = jobRepository;

            ViewBag.Subjects = subjectReference.GetAll();
        }

        public ActionResult Index()
        {
            var currentUserJobs = jobRepository.Get(orderByExpression: x => x.CreationTicks, sortOrder: SortOrder.Descending, limit: 100);
            return View(currentUserJobs);
        }

        public ActionResult Details(Guid id)
        {
            var job = jobRepository.Get(id);
            return View(new JobDetailsViewModel
            {
                Job = job,
                ChatIsVisible = !string.IsNullOrEmpty(job.TeacherUserId)
            });
        }
    }
}