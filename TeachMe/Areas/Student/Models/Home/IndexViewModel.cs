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
        private LoginViewModel loginViewModel;

        public IndexRecallViewModel[] Recalls { get { return recallData ?? (recallData = new IndexRecallViewModel[0]); } set { recallData = value; } }
        public LoginViewModel LoginViewModel { get { return loginViewModel ?? (loginViewModel = new LoginViewModel()); } set { loginViewModel = value; } }
    }
}