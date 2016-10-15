using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TeachMe.Models.Jobs;

namespace TeachMe.Areas.Teacher.Models.Jobs
{
    public class IndexAvailableViewModel
    {
        private Job[] jobs;

        public Job[] Jobs { get { return jobs ?? (jobs = new Job[0]); } set { jobs = value; } }

        [DisplayName("Только подходящие для Вас задачи")]
        public bool ShowOnlySuitableJobs { get; set; }
    }
}