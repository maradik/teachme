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
        private readonly IJobOpeningSpecification jobOpeningSpecification;

        public JobController(IJobRepository jobRepository,
                             IUploadedFileRepository uploadedFileRepository,
                             IUploadedFileConverter uploadFileConverter,
                             IJobAttachmentConverter attachmentConverter,
                             ISubjectReference subjectReference,
                             IJobActionService jobActionService,
                             IProjectTypeProvider projectTypeProvider,
                             IProjectInfoProvider projectInfoProvider,
                             IJobOpeningSpecification jobOpeningSpecification)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.jobRepository = jobRepository;
            this.uploadedFileRepository = uploadedFileRepository;
            this.uploadFileConverter = uploadFileConverter;
            this.attachmentConverter = attachmentConverter;
            this.jobActionService = jobActionService;
            this.jobOpeningSpecification = jobOpeningSpecification;

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
            return View(new JobDetailsViewModel {Job = job, JobAvailableActions = jobActionService.GetAvailableActions(job, ApplicationUser)});
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
            catch
            {
                return View();
            }
        }

        // GET: Job/Edit/5

        public ActionResult Edit(Guid id)
        {
            var job = jobActionService.DoAction(id, JobActionType.Edit, ApplicationUser);
            job.Attachments = job.Attachments.OrderByDescending(x => x.Type).ToList();
            return View(job);
        }

        // POST: Job/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Job jobViewModel, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
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

                if (!jobOpeningSpecification.IsSatisfiedBy(job))
                {
                    job.Status = JobStatus.Draft;
                }

                jobRepository.Write(job);

                return RedirectToAction("Details", new {job.Id});
            }
            catch
            {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoJobAction(Guid jobId, JobActionType jobActionType, string redirectActionName = nameof(Details))
        {
            if (jobActionType == JobActionType.Edit)
                return RedirectToAction(nameof(Edit), new { id = jobId });

            var updatedJob = jobActionService.DoAction(jobId, jobActionType, ApplicationUser);
            return updatedJob != null 
                ? RedirectToAction(redirectActionName, new {id = jobId})
                : RedirectToAction(nameof(Index));
        }

        public ActionResult _GetAvailableActions(Guid jobId)
        {
            var job = jobRepository.Get(jobId);
            var availableActions = jobActionService.GetAvailableActions(job, ApplicationUser);
            return Json(availableActions.Select(x => new { Value = (int)x, Text = x.GetHumanAnnotation() }), JsonRequestBehavior.AllowGet);
        }
    }
}