using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TeachMe.Models;
using TeachMe.Models.Users;

namespace TeachMe.Extensions
{
    public static class EntityWithUserIdExtensions
    {
        public static ApplicationUser GetUser(this IWithUserId entity)
        {
            return
                HttpContext.Current.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>()
                           .FindById(entity.UserId);
        }
    }
}