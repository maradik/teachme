﻿using System;
using TeachMe.Models.Jobs;

namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexJobViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SubjectTitle { get; set; }
        public double Cost { get; set; }
        public JobStatus Status { get; set; }
        public bool CanBeShown { get; set; }
    }
}