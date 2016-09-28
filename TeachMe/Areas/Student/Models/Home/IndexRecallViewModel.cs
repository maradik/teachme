using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexRecallViewModel
    {
        public string AuthorName { get; set; }
        public int Grade { get; set; } 
        public string Text { get; set; }
        public string PhotoUrl { get; set; }
    }
}