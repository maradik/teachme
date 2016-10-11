using System.Linq;
using TeachMe.Helpers.Settings;

namespace TeachMe.Services.Notifications
{
    public class SmsService : ISmsService
    {
        ICustomSmsService[] customSmsServices;

        public SmsService(ICustomSmsService[] customSmsServices)
        {
            this.customSmsServices = customSmsServices;
        }

        public void Send(string recipientPhone, string text)
        {
            if (ApplicationSettings.SmsNotificationEnabled)
            {
                customSmsServices.OrderByDescending(x => x.Priority).First().Send(recipientPhone, text);
            }
        }
    }
}