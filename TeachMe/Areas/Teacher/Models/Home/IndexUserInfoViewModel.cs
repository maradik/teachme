namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexUserInfoViewModel
    {
        private IndexUserJobViewModel[] jobs;
        private IndexUserSuitableJobViewModel[] suitableJobs;
        private IndexUserProfileViewModel profile;

        public IndexUserJobViewModel[] Jobs { get { return jobs ?? (jobs = new IndexUserJobViewModel[0]); } set { jobs = value; } }
        public IndexUserSuitableJobViewModel[] SuitableJobs { get { return suitableJobs ?? (suitableJobs = new IndexUserSuitableJobViewModel[0]); } set { suitableJobs = value; } }
        public IndexUserProfileViewModel Profile { get { return profile ?? (profile = new IndexUserProfileViewModel()); } set { profile = value; } }
    }
}