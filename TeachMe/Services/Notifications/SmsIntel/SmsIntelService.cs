using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TeachMe.Helpers.Settings;

namespace TeachMe.Services.Notifications.SmsAero
{
    public class SmsIntelService : ICustomSmsService
    {
        public int Priority => ApplicationSettings.SmsIntelServicePriority;

        // http://www.smsintel.ru/integration/
        public void Send(string recipientPhone, string text)
        {
            Task.Run(() => SendInternal(recipientPhone, text));
        }

        private void SendInternal(string recipientPhone, string text)
        {
            var url = BuildUrl(recipientPhone,
                               text,
                               ApplicationSettings.SmsIntelLogin,
                               ApplicationSettings.SmsIntelPassword,
                               ApplicationSettings.SmsIntelSenderName,
                               ApplicationSettings.SmsIntelUseAlfaSource,
                               ApplicationSettings.SmsIntelChannel);
            var request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseString = Regex.Unescape(new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd());

                if (response.StatusCode != HttpStatusCode.OK ||
                    responseString.IndexOf("\"code\":1", StringComparison.InvariantCultureIgnoreCase) < 0)
                {
                    throw new Exception($"Не удалось отправить смс на номер {recipientPhone} с текстом: {text}. Код ответа {response.StatusCode}, тело: `{responseString}`. Url запроса: {url}");
                }
            }
        }

        private string BuildUrl(string recipientPhone, string text, string login, string password, string senderName, int useAlfaSource, int channel)
        {
            var url = $"https://lcab.smsintel.ru/lcabApi/sendSms.php";
            url += $"?send=1&login={HttpUtility.UrlEncode(login)}&password={HttpUtility.UrlEncode(password)}";
            url += $"&to={HttpUtility.UrlEncode(recipientPhone)}&txt={HttpUtility.UrlEncode(text)}";

            if (!string.IsNullOrEmpty(senderName))
                url += $"&source={senderName}";

            url += $"&use_alfasource={useAlfaSource}&channel={channel}";

            return url;
        }
    }
}