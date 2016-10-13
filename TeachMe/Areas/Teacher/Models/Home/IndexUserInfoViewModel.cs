namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexUserInfoViewModel
    {
        private IndexUserJobViewModel[] jobs;

        public IndexUserJobViewModel[] Jobs { get { return jobs ?? (jobs = new IndexUserJobViewModel[0]); } set { jobs = value; } }
    }
}