using System.Linq;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.Services.JobChat;

namespace TeachMe.ViewModels.JobChat
{
    public class JobMessageViewModelFactory : IJobMessageViewModelFactory
    {
        private readonly IJobMessageAttachmentAccessService attachmentAccessService;

        public JobMessageViewModelFactory(IJobMessageAttachmentAccessService attachmentAccessService)
        {
            this.attachmentAccessService = attachmentAccessService;
        }

        public JobMessageViewModel Create(JobMessage message, ApplicationUser user)
        {
            var hasAccessToAttachments = attachmentAccessService.HasAccess(message, user);
            return new JobMessageViewModel
            {
                Id = message.Id,
                JobId = message.JobId,
                Text = message.Text,
                UserId = message.UserId,
                Attachments = message.Attachments.Select(x => Create(message, x, hasAccessToAttachments)).ToArray(),
                CreationTicks = message.CreationTicks
            };
        }

        private IJobMessageAttachmentViewModel Create(JobMessage message, JobAttachment attachment, bool hasAccessToAttachments)
        {
            return hasAccessToAttachments
                ? (IJobMessageAttachmentViewModel) new JobMessageAttachmentViewModel(attachment, message.Id)
                : (IJobMessageAttachmentViewModel) new DummyJobMessageAttachmentViewModel(attachment);
        }
    }
}