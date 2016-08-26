using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeachMe.DataAccess;
using TeachMe.Models;
using TeachMe.ProjectsSupport;

namespace TeachMe.Controllers
{
    public class JobChatController : ControllerBase
    {
        private readonly IJobRepository jobRepository;
        private readonly IJobMessageRepository jobMessageRepository;

        public JobChatController(IProjectTypeProvider projectTypeProvider,
                                 IJobRepository jobRepository,
                                 IJobMessageRepository jobMessageRepository)
            : base(projectTypeProvider)
        {
            this.jobRepository = jobRepository;
            this.jobMessageRepository = jobMessageRepository;
        }

        public PartialViewResult _GetMessages(Guid jobId, long afterTicks = 0)
        {
            AssertUserHasAccessToJobChat(jobId);
            var messages = jobMessageRepository.GetAllByJobIdCreatedAfter(jobId, afterTicks);
            return PartialView(messages);
        }

        public PartialViewResult _CreateMessage(Guid jobId)
        {
            AssertUserHasAccessToJobChat(jobId);
            ViewData.TemplateInfo.HtmlFieldPrefix = nameof(JobChatController);
            return PartialView(new JobMessage {JobId = jobId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _CreateMessage([Bind(Prefix = nameof(JobChatController))] JobMessage jobMessage,
                                                [Bind(Prefix = nameof(JobChatController))] IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            if (ModelState.IsValid)
            {
                AssertUserHasAccessToJobChat(jobMessage.JobId);

                jobMessage.UserId = ApplicationUser.Id;
                jobMessageRepository.Write(jobMessage);

                return PartialView(new JobMessage { JobId = jobMessage.JobId });
            }

            return PartialView();
        }

        private void AssertUserHasAccessToJobChat(Guid jobId)
        {
            var job = jobRepository.Get(jobId);

            if (job.TeacherUserId != ApplicationUser.Id && job.StudentUserId != ApplicationUser.Id)
                throw new HttpException((int) HttpStatusCode.Forbidden,
                                        $"У пользователя {ApplicationUser.UserName} нет доступа к чату задачи {job.Id}");
        }
    }
}