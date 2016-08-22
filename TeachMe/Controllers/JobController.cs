using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeachMe.Converters;
using TeachMe.DataAccess;
using TeachMe.Models;
using TeachMe.ProjectsSupport;

namespace TeachMe.Controllers
{
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly JobRepository jobRepository;
        private readonly IUploadedFileRepository uploadedFileRepository;
        private readonly IUploadedFileConverter uploadFileConverter;
        private readonly IJobAttachmentConverter attachmentConverter;

        public JobController(JobRepository jobRepository,
                             IUploadedFileRepository uploadedFileRepository,
                             IUploadedFileConverter uploadFileConverter,
                             IJobAttachmentConverter attachmentConverter,
                             IProjectTypeProvider projectTypeProvider)
            : base(projectTypeProvider)
        {
            this.jobRepository = jobRepository;
            this.uploadedFileRepository = uploadedFileRepository;
            this.uploadFileConverter = uploadFileConverter;
            this.attachmentConverter = attachmentConverter;
        }

        // GET: Job

        public ActionResult Index()
        {
            return View();
        }

        // GET: Job/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Job/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: Job/Create

        [HttpPost]
        public ActionResult Create(Job job, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var uploadedJobAttachments = uploadedFiles.Where(x => x != null).Select(uploadFileConverter.ToUploadedJobAttachment).ToArray();
                uploadedFileRepository.Save(uploadedJobAttachments);

                job.Attachments = uploadedJobAttachments.Select(attachmentConverter.FromUploadedJobAttachment).ToList();
                jobRepository.Write(job);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Job/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Job/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Job/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Job/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}