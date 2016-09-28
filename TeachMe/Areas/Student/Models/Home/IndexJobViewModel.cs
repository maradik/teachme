using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.Models.Jobs;
using TeachMe.References;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexJobViewModel
    {
        public string Title { get; set; }
        public string SubjectTitle { get; set; }
        public double Cost { get; set; }
        public JobStatus Status { get; set; }
    }
}