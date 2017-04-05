using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Microsoft.AspNet.Identity;
using TeachMe.Converters;
using TeachMe.DataAccess.FileUploading;
using TeachMe.DataAccess.Jobs;
using TeachMe.Models.Jobs;
using TeachMe.ProjectsSupport;
using TeachMe.References;
using TeachMe.Services.Jobs;
using TeachMe.ViewModels.Jobs;
using TeachMe.Helpers.Settings;
using TeachMe.Services.General;
using TeachMe.Extensions;
using TeachMe.Services.Jobs.JobActionHandlers;

namespace TeachMe.Areas.Student.Controllers
{
    [Authorize]
    public class JobController : StudentControllerBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(JobController));

        private readonly IJobRepository jobRepository;
        private readonly IUploadedFileRepository uploadedFileRepository;
        private readonly IUploadedFileConverter uploadFileConverter;
        private readonly IJobAttachmentConverter attachmentConverter;
        private readonly IJobActionService jobActionService;

        public JobController(IJobRepository jobRepository,
                             IUploadedFileRepository uploadedFileRepository,
                             IUploadedFileConverter uploadFileConverter,
                             IJobAttachmentConverter attachmentConverter,
                             ISubjectReference subjectReference,
                             IJobActionService jobActionService,
                             IProjectTypeProvider projectTypeProvider,
                             IProjectInfoProvider projectInfoProvider)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.jobRepository = jobRepository;
            this.uploadedFileRepository = uploadedFileRepository;
            this.uploadFileConverter = uploadFileConverter;
            this.attachmentConverter = attachmentConverter;
            this.jobActionService = jobActionService;

            ViewBag.Subjects = subjectReference.GetAll();
        }

        // GET: Job

        public ActionResult Index()
        {
            var currentUserJobs = jobRepository.GetAllByStudentUserId(ApplicationUser.Id).OrderByDescending(x => x.CreationTicks).ToArray();
            return View(currentUserJobs);
        }

        // GET: Job/Details/5

        public ActionResult Details(Guid id)
        {
            var job = jobRepository.GetByIdAndStudentUserId(id, ApplicationUser.Id);

            if (job.Status == JobStatus.FinishedWithRemainAmountNeeded && 
                jobActionService.GetAvailableActions(job, ApplicationUser).Contains(JobActionType.ReserveRemainAmount))
            {
                jobActionService.DoAction(job.Id, JobActionType.ReserveRemainAmount, ApplicationUser);
                return RedirectToAction(nameof(Details), new {id});
            }

            return View(new JobDetailsViewModel
            {
                Job = job,
                JobAvailableActions = jobActionService.GetAvailableActions(job, ApplicationUser),
                ChatIsVisible = !string.IsNullOrEmpty(job.TeacherUserId)
            });
        }

        // GET: Job/Create

        public ActionResult Create()
        {
            return View(new Job { StudentCost = 100 });
        }

        // POST: Job/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Job job, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (uploadedFiles != null)
                {
                    var uploadedJobAttachments = uploadedFiles.Where(x => x != null).Select(uploadFileConverter.ToUploadedJobAttachment).ToArray();
                    uploadedFileRepository.Save(uploadedJobAttachments);
                    job.Attachments = uploadedJobAttachments.Select(attachmentConverter.FromUploadedJobAttachment).ToList();
                }

                job.StudentUserId = User.Identity.GetUserId();
                job.TeacherUserId = string.Empty;
                job.Status = JobStatus.Draft;
                job.CommissionRate = ApplicationSettings.JobCommissionRate;
                jobRepository.Write(job);

                try
                {
                    if (jobActionService.GetAvailableActions(job, ApplicationUser).Contains(JobActionType.Open))
                    {
                        job = jobActionService.DoAction(job.Id, JobActionType.Open, ApplicationUser);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error($"Не удалось опубликовать задачу Id={job.Id} при создании.", e);
                }

                return RedirectToAction("Details", new {job.Id});
            }
            catch (Exception e)
            {
                Logger.Error("Не удалось создать задачу", e);
                return View();
            }
        }

        // GET: Job/Edit/5

        public ActionResult Edit(Guid id)
        {
            ViewBag.JobId = id; // костыль для EditorTemplates/JobAttachment.cshtml
            var job = jobActionService.DoAction(id, JobActionType.Edit, ApplicationUser);
            job.Attachments = job.Attachments.OrderByDescending(x => x.Type).ToList();
            return View(job);
        }

        // POST: Job/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Job jobViewModel, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            ViewBag.JobId = id; // костыль для EditorTemplates/JobAttachment.cshtml
            var job = jobActionService.DoAction(id, JobActionType.Edit, ApplicationUser);

            try
            {
                if (!ModelState.IsValid)
                {
                    job.Attachments = job.Attachments.OrderByDescending(x => x.Type).ToList();
                    return View(job);
                }

                JobAttachment[] deletedAttachments;
                UpdateJobFromViewModel(job, jobViewModel, out deletedAttachments);

                if (deletedAttachments != null && deletedAttachments.Length > 0)
                {
                    uploadedFileRepository.Delete(deletedAttachments.Select(x => x.FileName).ToArray());
                }

                if (uploadedFiles != null)
                {
                    var uploadedJobAttachments = uploadedFiles.Where(x => x != null).Select(uploadFileConverter.ToUploadedJobAttachment).ToArray();
                    uploadedFileRepository.Save(uploadedJobAttachments);
                    job.Attachments.AddRange(uploadedJobAttachments.Select(attachmentConverter.FromUploadedJobAttachment));
                }

                jobRepository.Write(job);

                return RedirectToAction("Details", new {job.Id});
            }
            catch (Exception e)
            {
                Logger.Error($"Не удалось изменить задачу JobId={id}", e);
                job.Attachments = job.Attachments.OrderByDescending(x => x.Type).ToList();
                return View(job);
            }
        }

        private static void UpdateJobFromViewModel(Job job, Job jobViewModel, out JobAttachment[] deletedAttachments)
        {
            job.SubjectId = jobViewModel.SubjectId;
            job.Description = jobViewModel.Description;
            job.Title = jobViewModel.Title;
            job.StudentCost = jobViewModel.StudentCost;

            var jobViewModelFileNames = new HashSet<string>(jobViewModel.Attachments.Select(y => y.FileName));
            deletedAttachments = job.Attachments.Where(x => !jobViewModelFileNames.Contains(x.FileName)).ToArray();
            job.Attachments = job.Attachments.Where(x => jobViewModelFileNames.Contains(x.FileName)).ToList();
        }


        public FileResult DownloadAttachment(Guid jobId, Guid attachmentId)
        {
            var job = jobRepository.Get(jobId);
            var attachment = job.Attachments.Single(x => x.Id == attachmentId);
            return attachmentConverter.ToFileResult(attachment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _DoJobAction(Guid jobId, JobActionType jobActionType)
        {
            if (jobActionType == JobActionType.Edit)
                return Json(new JobActionResult { RedirectUrl = Url.Action(nameof(Edit), new { id = jobId }) });

            Job updatedJob;
            try
            {
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

            return Json(new JobActionResult { RedirectUrl = updatedJob == null ? Url.Action(nameof(Index)) : ""});
        }

        public ActionResult _GetAvailableActions(Guid jobId)
        {
            var job = jobRepository.Get(jobId);
            var availableActions = jobActionService.GetAvailableActions(job, ApplicationUser);
            return Json(availableActions.Select(x => new { Value = (int)x, Text = x.GetHumanAnnotation() }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetStatus(Guid id)
        {
            var job = jobRepository.GetByIdAndStudentUserId(id, ApplicationUser.Id);
            return Json((int) job.Status, JsonRequestBehavior.AllowGet);
        }
    }
}