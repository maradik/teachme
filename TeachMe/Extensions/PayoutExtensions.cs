using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TeachMe.Models.Jobs;
using TeachMe.Models.Payouts;
using TeachMe.Models.Users;

namespace TeachMe.Extensions
{
    public static class PayoutExtensions
    {
        public static ApplicationUser GetUser(this Payout payout)
        {
            return
                HttpContext.Current.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>()
                           .FindById(payout.UserId);
        }
    }
}