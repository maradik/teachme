using System.Web.Mvc;

namespace TeachMe.Areas.Teacher
{
    public class TeacherAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Teacher";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Teacher_default",
                "Teacher/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional},
                new[] {"TeachMe.Areas.Teacher.Controllers"}
                );
        }
    }
}