using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Jobs;
using TeachMe.Extensions;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;
using TeachMe.References;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexUserJobMessagesViewModelProvider
    {
        private readonly IJobRepository jobRepository;
        private readonly IJobMessageRepository jobMessageRepository;

        public IndexUserJobMessagesViewModelProvider(IJobRepository jobRepository, IJobMessageRepository jobMessageRepository)
        {
            this.jobRepository = jobRepository;
            this.jobMessageRepository = jobMessageRepository;
        }

        public IndexUserJobMessageViewModel[] Get(ApplicationUser user, int count = 10)
        {
            var userJobs = jobRepository.Get(x => x.StudentUserId == user.Id).ToDictionary(x => x.Id);
            var userJobIds = userJobs.Keys.ToArray();
            var jobMessages = jobMessageRepository.Get(x => userJobIds.Contains(x.JobId) && x.UserId != user.Id, x => x.CreationTicks, SortOrder.Descending, count);

            return jobMessages.Select(x =>
                                      new IndexUserJobMessageViewModel
                                      {
                                          JobId = x.JobId,
                                          Text = x.Text,
                                          AuthorName = x.GetUser().UserName,
                                          HasAttachments = x.Attachments.Length > 0,
                                          CreationTicks = x.CreationTicks,
                                          JobTitle = userJobs[x.JobId].Title
                                      }
                ).ToArray();
        }
    }
}