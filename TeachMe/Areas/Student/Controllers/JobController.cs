﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TeachMe.Converters;
using TeachMe.DataAccess;
using TeachMe.DataAccess.FileUploading;
using TeachMe.DataAccess.Jobs;
using TeachMe.Models;
using TeachMe.Models.Jobs;
using TeachMe.ProjectsSupport;
using TeachMe.References;
using TeachMe.Services.Jobs;
using TeachMe.ViewModels.Jobs;
using TeachMe.Helpers.Settings;

namespace TeachMe.Areas.Student.Controllers
{
    [Authorize]
    public class JobController : StudentControllerBase
    {
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
                             IJobOpeningSpecification jobOpeningSpecification)
            : base(projectTypeProvider)
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
            return View(new Job());
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
                job.Status = jobOpeningSpecification.IsSatisfiedBy(job) ? JobStatus.Opened : JobStatus.Draft;
                job.CommissionRate = ApplicationSettings.JobCommissionRate;
                jobRepository.Write(job);

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
            var job = jobRepository.GetByIdAndStudentUserId(id, ApplicationUser.Id);
            AssertJobMayBeEdited(job);
            return View(job);
        }

        // POST: Job/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Job jobViewModel, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            var job = jobRepository.GetByIdAndStudentUserId(id, ApplicationUser.Id);
            AssertJobMayBeEdited(job);

            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
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
                return View();
            }
        }

        // GET: Job/Delete/5

        public ActionResult Delete(Guid id)
        {
            var job = jobRepository.GetByIdAndStudentUserId(id, ApplicationUser.Id);
            return View(job);
        }

        // POST: Job/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            var job = jobRepository.GetByIdAndStudentUserId(id, ApplicationUser.Id);

            try
            {
                AssertJobMayBeDeleted(job);

                jobRepository.Remove(job.Id);

                return RedirectToAction("Index");
            }
            catch
            {
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

        private static void AssertJobMayBeDeleted(Job job)
        {
            if (!string.IsNullOrEmpty(job.TeacherUserId))
                throw new InvalidOperationException($"Невозможно удалить Job, за которым закреплен исполнитель (JobId = {job.Id})");
        }

        private static void AssertJobMayBeEdited(Job job)
        {
            if (!string.IsNullOrEmpty(job.TeacherUserId))
                throw new InvalidOperationException($"Невозможно изменить Job, за которым закреплен исполнитель (JobId = {job.Id})");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoJobAction(Guid jobId, JobActionType jobActionType)
        {
            jobActionService.DoAction(jobId, jobActionType, ApplicationUser);
            return RedirectToAction(nameof(Details), new {id = jobId});
        }
    }
}