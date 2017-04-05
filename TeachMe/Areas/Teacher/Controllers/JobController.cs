using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeachMe.Areas.Teacher.Models.Jobs;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.Extensions;
using TeachMe.Models.Jobs;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;
using TeachMe.Services.Jobs;
using TeachMe.ViewModels.Jobs;
using System.Linq.Expressions;
using log4net;
using TeachMe.Converters;

namespace TeachMe.Areas.Teacher.Controllers
{
    public class JobController : TeacherControllerBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(JobController));

        private readonly IJobActionService jobActionService;
        private readonly IJobRepository jobRepository;
        private readonly IJobAttachmentConverter attachmentConverter;

        public JobController(IProjectTypeProvider projectTypeProvider,
                             IProjectInfoProvider projectInfoProvider,
                             IJobActionService jobActionService,
                             IJobRepository jobRepository,
                             IJobAttachmentConverter attachmentConverter)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.jobActionService = jobActionService;
            this.jobRepository = jobRepository;
            this.attachmentConverter = attachmentConverter;
        }

        public ActionResult Index()
        {
            var jobs = jobRepository.GetAllByTeacherUserId(ApplicationUser.Id).OrderByDescending(x => x.CreationTicks).ToArray();
            return View(jobs);
        }

        public ActionResult IndexAvailable(bool showOnlySuitableJobs = false)
        {
            Expression<Func<Job, bool>> whereExpression;

            if (showOnlySuitableJobs)
            {
                whereExpression = x => x.Status == JobStatus.Opened && ApplicationUser.SubjectIds.Contains(x.SubjectId);
            }
            else
            {
                whereExpression = x => x.Status == JobStatus.Opened;
            }

            var jobs = jobRepository.Get(whereExpression, x => x.CreationTicks, SortOrder.Descending);
            var viewModel = new IndexAvailableViewModel { Jobs = jobs, ShowOnlySuitableJobs = showOnlySuitableJobs };
            return View(viewModel);
        }

        [AllowAnonymous]
        public ActionResult DetailsForAnonymous(Guid id)
        {
            if (Request.IsAuthenticated)
            {
                Logger.Warn($"Метод {nameof(DetailsForAnonymous)} контроллера {typeof(JobController)} недоступен авторизованным пользователям");
                return RedirectToAction("Details", new {id});
            }

            var job = jobRepository.Get(id);

            if (job?.Status != JobStatus.Opened)
            {
                Logger.Warn($"Невозможно отобразить задачу {id} анонимному пользователю, т.к. она уже в работе");
                return RedirectToAction("Index", "Home");
            }

            return View(new JobDetailsForAnonymousViewModel
            {
                Job = job
            });
        }

        public ActionResult Details(Guid id)
        {
            var job = jobRepository.Get(id);

            return View(new JobDetailsViewModel
            {
                Job = job,
                JobAvailableActions = jobActionService.GetAvailableActions(job, ApplicationUser),
                ChatIsVisible = !string.IsNullOrEmpty(job.TeacherUserId) && job.TeacherUserId == ApplicationUser.Id
            });
        }

        public FileResult DownloadAttachment(Guid jobId, Guid attachmentId)
        {
            var job = jobRepository.Get(jobId);
            var attachment = job.Attachments.Single(x => x.Id == attachmentId);
            return attachmentConverter.ToFileResult(attachment);
        }

        [HttpPost]
        public ActionResult _DoJobAction(Guid jobId, JobActionType jobActionType)
        {
            try
            {
                jobActionService.DoAction(jobId, jobActionType, ApplicationUser);
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
            return Json(new JobActionResult());
        }

        public ActionResult _GetAvailableActions(Guid jobId)
        {
            var job = jobRepository.Get(jobId);
            var availableActions = jobActionService.GetAvailableActions(job, ApplicationUser);
            return Json(availableActions.Select(x => new { Value = (int)x, Text = x.GetHumanAnnotation() }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetStatus(Guid id)
        {
            var job = jobRepository.GetByIdAndTeacherUserId(id, ApplicationUser.Id);
            return Json((int)job.Status, JsonRequestBehavior.AllowGet);
        }
    }
}