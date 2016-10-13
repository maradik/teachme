namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexUserInfoViewModel
    {
        private IndexUserJobViewModel[] jobs;
        private IndexUserSuitableJobViewModel[] suitableJobs;

        public IndexUserJobViewModel[] Jobs { get { return jobs ?? (jobs = new IndexUserJobViewModel[0]); } set { jobs = value; } }
        public IndexUserSuitableJobViewModel[] SuitableJobs { get { return suitableJobs ?? (suitableJobs = new IndexUserSuitableJobViewModel[0]); } set { suitableJobs = value; } }
    }
}