using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.Models.Users;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexViewModel
    {
        private IndexRecallViewModel[] recallData;
        private IndexJobViewModel[] jobs;
        private LoginViewModel loginViewModel;
        private IndexUserInfoViewModel userInfo;

        public IndexRecallViewModel[] Recalls { get { return recallData ?? (recallData = new IndexRecallViewModel[0]); } set { recallData = value; } }
        public IndexJobViewModel[] Jobs { get { return jobs ?? (jobs = new IndexJobViewModel[0]); } set { jobs = value; } }
        public IndexUserInfoViewModel UserInfo { get { return userInfo ?? (userInfo = new IndexUserInfoViewModel()); } set { userInfo = value; } }
        public LoginViewModel LoginViewModel { get { return loginViewModel ?? (loginViewModel = new LoginViewModel()); } set { loginViewModel = value; } }
    }
}