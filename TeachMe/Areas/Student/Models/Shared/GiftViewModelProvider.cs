using System.Web;
using TeachMe.Helpers.Settings;

namespace TeachMe.Areas.Student.Models.Shared
{
    public class GiftViewModelProvider
    {
        public GiftViewModel Get()
        {
            var promoModalHiddenCookie = HttpContext.Current.Request.Cookies["promoModalHidden"];
            bool promoModalHidden;
            bool.TryParse(promoModalHiddenCookie?.Value, out promoModalHidden);

            return new GiftViewModel
            {
                GiftAmountForNewUser = (int)ApplicationSettings.StudentInitialCash,
                NeedShowPromoModal = (int)ApplicationSettings.StudentInitialCash > 0 && !promoModalHidden
            };
        }
    }
}