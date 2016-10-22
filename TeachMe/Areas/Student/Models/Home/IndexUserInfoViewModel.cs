using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexUserInfoViewModel
    {
        private IndexUserJobViewModel[] jobs;
        private IndexUserJobMessageViewModel[] jobMessages;
        private IndexUserProfileViewModel profile;

        public IndexUserJobViewModel[] Jobs { get { return jobs ?? (jobs = new IndexUserJobViewModel[0]); } set { jobs = value; } }
        public IndexUserJobMessageViewModel[] JobMessages { get { return jobMessages ?? (jobMessages = new IndexUserJobMessageViewModel[0]); } set { jobMessages = value; } }
        public IndexUserProfileViewModel Profile { get { return profile ?? (profile = new IndexUserProfileViewModel()); } set { profile = value; } }
    }
}