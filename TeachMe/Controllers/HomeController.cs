﻿using System.Web.Mvc;
using TeachMe.ProjectsSupport;

namespace TeachMe.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IProjectTypeProvider projectTypeProvider)
            : base(projectTypeProvider)
        {
        }

        public ActionResult Index()
        {
            return ProjectType == ProjectType.Student
                       ? RedirectToAction("Index", "Home", new {area = "Student"})
                       : RedirectToAction("Index", "Home", new {area = "Teacher"});
        }

        public ActionResult About()
        {
            return ProjectType == ProjectType.Student
                       ? RedirectToAction("About", "Home", new { area = "Student" })
                       : RedirectToAction("About", "Home", new { area = "Teacher" });
        }

        public ActionResult Contact()
        {
            return ProjectType == ProjectType.Student
                       ? RedirectToAction("Contact", "Home", new { area = "Student" })
                       : RedirectToAction("Contact", "Home", new { area = "Teacher" });
        }
    }
}