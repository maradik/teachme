using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexUserInfoViewModel
    {
        private IndexUserJobViewModel[] jobs;

        public IndexUserJobViewModel[] Jobs { get { return jobs ?? (jobs = new IndexUserJobViewModel[0]); } set { jobs = value; } }
    }
}