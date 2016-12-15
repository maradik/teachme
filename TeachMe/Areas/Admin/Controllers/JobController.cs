using System;
using System.Web.Mvc;
using log4net;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.Extensions;
using TeachMe.Models.Jobs;
using TeachMe.ProjectsSupport;
using TeachMe.References;
using TeachMe.Services.General;
using TeachMe.Services.Jobs;
using TeachMe.ViewModels.Jobs;
using JobDetailsViewModel = TeachMe.Areas.Admin.Models.Jobs.JobDetailsViewModel;

namespace TeachMe.Areas.Admin.Controllers
{
    public class JobController : AdminControllerBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(JobController));

        private readonly IJobRepository jobRepository;
        private readonly IJobActionService jobActionService;

        public JobController(IJobRepository jobRepository,
                             ISubjectReference subjectReference,
                             IJobActionService jobActionService,
                             IProjectTypeProvider projectTypeProvider,
                             IProjectInfoProvider projectInfoProvider)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.jobRepository = jobRepository;
            this.jobActionService = jobActionService;

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
                JobAvailableActions = jobActionService.GetAvailableActions(job, ApplicationUser),
                ChatIsVisible = !string.IsNullOrEmpty(job.TeacherUserId)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _DoJobAction(Guid jobId, JobActionType jobActionType)
        {
            Job updatedJob;
            try
            {
                if (jobActionType == JobActionType.Edit)
                    throw new NotImplementedException($"Действие {jobActionType} не реализовано");

                updatedJob = jobActionService.DoAction(jobId, jobActionType, ApplicationUser);
            }
            catch (InvalidJobActionException)
            {
                return Json(new JobActionResult
                {
                    HasErrors = true,
                    ErrorMessage = $"Не удалось выполнить действие \"{jobActionType.GetHumanAnnotation()}\", т.к. другой пользователь изменил состояние задачи."
                });
            }
            catch (Exception e)
            {
                Logger.Error($"Не удалось применить действие {jobActionType} к задаче {jobId}", e);
                return Json(new JobActionResult
                {
                    HasErrors = true,
                    ErrorMessage = $"Неопознанная ошибка! Не удалось выполнить действие \"{jobActionType.GetHumanAnnotation()}\".",
                    RedirectUrl = Url.Action(nameof(Index))
                });
            }

            return Json(new JobActionResult { RedirectUrl = updatedJob == null ? Url.Action(nameof(Index)) : "" });
        }
    }
}