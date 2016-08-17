using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}