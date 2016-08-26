using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TeachMe.Models;

namespace TeachMe.Extensions
{
    public static class JobMessageExtensions
    {
        public static ApplicationUser GetUser(this JobMessage message)
        {
            return
                HttpContext.Current.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>()
                           .FindById(message.UserId);
        }
    }
}