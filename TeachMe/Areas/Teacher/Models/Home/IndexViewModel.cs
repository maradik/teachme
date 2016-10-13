using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.Models.Users;

namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexViewModel
    {
        private IndexRecallViewModel[] recallData;
        private LoginViewModel loginViewModel;
        private IndexJobViewModel[] jobs;

        public IndexRecallViewModel[] Recalls { get { return recallData ?? (recallData = new IndexRecallViewModel[0]); } set { recallData = value; } }
        public LoginViewModel LoginViewModel { get { return loginViewModel ?? (loginViewModel = new LoginViewModel()); } set { loginViewModel = value; } }
        public IndexJobViewModel[] Jobs { get { return jobs ?? (jobs = new IndexJobViewModel[0]); } set { jobs = value; } }
    }
}