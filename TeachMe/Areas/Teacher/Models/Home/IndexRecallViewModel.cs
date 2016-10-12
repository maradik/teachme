using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexRecallViewModel
    {
        public string AuthorName { get; set; }
        public string Text { get; set; }
        public string PhotoUrl { get; set; }
        public string Subject { get; set; }
        public string City { get; set; }
    }
}