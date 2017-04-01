using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using log4net;
using MongoDB.Bson.Serialization.Conventions;
using TeachMe.Converters;
using TeachMe.DataAccess.FileUploading;
using TeachMe.DataAccess.Jobs;
using TeachMe.Extensions;
using TeachMe.Models.Jobs;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;

namespace TeachMe.Controllers
{
    [Authorize]
    public class JobChatController : ControllerBase
    {
        private const string ModelBindingPrefix = nameof(JobChatController);
        private static readonly ILog Logger = LogManager.GetLogger(typeof(JobChatController));

        private readonly IJobRepository jobRepository;
        private readonly IJobMessageRepository jobMessageRepository;
        private readonly IUploadedFileConverter uploadedFileConverter;
        private readonly IJobAttachmentConverter attachmentConverter;
        private readonly IUploadedFileRepository uploadedFileRepository;

        public JobChatController(IProjectTypeProvider projectTypeProvider,
                                 IProjectInfoProvider projectInfoProvider,
                                 IJobRepository jobRepository,
                                 IJobMessageRepository jobMessageRepository,
                                 IUploadedFileConverter uploadedFileConverter,
                                 IJobAttachmentConverter attachmentConverter,
                                 IUploadedFileRepository uploadedFileRepository)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.jobRepository = jobRepository;
            this.jobMessageRepository = jobMessageRepository;
            this.uploadedFileConverter = uploadedFileConverter;
            this.attachmentConverter = attachmentConverter;
            this.uploadedFileRepository = uploadedFileRepository;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewData.TemplateInfo.HtmlFieldPrefix = ModelBindingPrefix;
        }

        public FileResult DownloadAttachment(Guid messageId, Guid attachmentId)
        {
            var jobMessage = jobMessageRepository.Get(messageId);
            AssertUserHasAccessToJobChat(jobMessage.JobId);
            var attachment = jobMessage.Attachments.Single(x => x.Id == attachmentId);
            return attachmentConverter.ToFileResult(attachment);
        }

        public PartialViewResult _GetMessages(Guid jobId, long afterTicks = 0)
        {
            AssertUserHasAccessToJobChat(jobId);
            var messages = jobMessageRepository.GetAllByJobIdCreatedAfter(jobId, afterTicks).OrderBy(x => x.CreationTicks).ToArray();
            return PartialView(messages);
        }

        public PartialViewResult _CreateMessage(Guid jobId)
        {
            AssertUserHasAccessToJobChat(jobId);
            return PartialView(CreateJobMessage(jobId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _CreateMessage([Bind(Prefix = ModelBindingPrefix)] JobMessage jobMessage,
                                                [Bind(Prefix = ModelBindingPrefix + ".uploadedFiles")] IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            AssertUserHasAccessToJobChat(jobMessage.JobId);

            if (!ModelState.IsValid)
            {
                return PartialView();
            }

            try
            {
                if (uploadedFiles != null)
                {
                    var uploadedJobAttachments = uploadedFiles.Where(x => x != null).Select(uploadedFileConverter.ToUploadedJobAttachment).ToArray();
                    uploadedFileRepository.Save(uploadedJobAttachments);
                    jobMessage.Attachments = uploadedJobAttachments.Select(attachmentConverter.FromUploadedJobAttachment).ToArray();
                }

                jobMessage.UserId = ApplicationUser.Id;
                jobMessageRepository.Write(jobMessage);

                ModelState.Clear();
                return PartialView(CreateJobMessage(jobMessage.JobId));
            }
            catch (Exception e)
            {
                Logger.Error("Не удалось создать сообщение", e);
                return PartialView();
            }
        }

        private static JobMessage CreateJobMessage(Guid jobId)
        {
            return new JobMessage { JobId = jobId };
        }

        private void AssertUserHasAccessToJobChat(Guid jobId)
        {
            var job = jobRepository.Get(jobId);

            if (job.TeacherUserId != ApplicationUser.Id && job.StudentUserId != ApplicationUser.Id && !ApplicationUser.IsAdmin())
                throw new HttpException((int) HttpStatusCode.Forbidden,
                                        $"У пользователя {ApplicationUser.UserName} нет доступа к чату задачи {job.Id}");
        }
    }
}