using System.Web;
using TeachMe.Helpers.Settings;

namespace TeachMe.Areas.Student.Models.Shared
{
    public class GiftViewModel
    {
        public int GiftAmountForNewUser { get; set; }
        public bool NeedShowPromoModal { get; set; }
    }
}