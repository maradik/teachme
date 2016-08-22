using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TeachMe.Converters;
using TeachMe.DataAccess;
using TeachMe.Models;
using TeachMe.ProjectsSupport;
using TeachMe.References;
using ControllerBase = TeachMe.Controllers.ControllerBase;

namespace TeachMe.Areas.Student.Controllers
{
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly JobRepository jobRepository;
        private readonly IUploadedFileRepository uploadedFileRepository;
        private readonly IUploadedFileConverter uploadFileConverter;
        private readonly IJobAttachmentConverter attachmentConverter;
        private readonly ISubjectReference subjectReference;

        public JobController(JobRepository jobRepository,
                             IUploadedFileRepository uploadedFileRepository,
                             IUploadedFileConverter uploadFileConverter,
                             IJobAttachmentConverter attachmentConverter,
                             ISubjectReference subjectReference,
                             IProjectTypeProvider projectTypeProvider)
            : base(projectTypeProvider)
        {
            this.jobRepository = jobRepository;
            this.uploadedFileRepository = uploadedFileRepository;
            this.uploadFileConverter = uploadFileConverter;
            this.attachmentConverter = attachmentConverter;
            this.subjectReference = subjectReference;
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
            ViewBag.Subjects = subjectReference.GetAll();
            return View(new Job());
        }

        // POST: Job/Create

        [HttpPost]
        public ActionResult Create(Job job, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            ViewBag.Subjects = subjectReference.GetAll();

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