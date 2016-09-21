using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using TeachMe.Helpers.Settings;

namespace TeachMe.Services.Notifications.SmsAero
{
    public class SmsAeroService : ICustomSmsService
    {
        public int Priority => ApplicationSettings.SmsAeroServicePriority;

        // https://smsaero.ru/api/description/
        public void Send(string recipientPhone, string text)
        {
            Task.Run(() => SendInternal(recipientPhone, text));
        }

        private void SendInternal(string recipientPhone, string text)
        {
            var url = BuildUrl(recipientPhone,
                               text,
                               ApplicationSettings.SmsAeroLogin,
                               ApplicationSettings.SmsAeroApiKey,
                               ApplicationSettings.SmsAeroSenderName,
                               ApplicationSettings.SmsAeroDigital,
                               ApplicationSettings.SmsAeroType);
            var request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (response.StatusCode != HttpStatusCode.OK ||
                    responseString.IndexOf("reject", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    throw new Exception($"Не удалось отправить смс на номер {recipientPhone} с текстом: {text}. Код ответа {response.StatusCode}, тело: `{responseString}`. Url запроса: {url}");
                }
            }
        }

        private string BuildUrl(string recipientPhone, string text, string user, string apiKey, string senderName, int digital, int type)
        {
            var url = $"https://gate.smsaero.ru/send/";
            url += $"?send=1&user={HttpUtility.UrlEncode(user)}&password={HttpUtility.UrlEncode(apiKey)}";
            url += $"&to={HttpUtility.UrlEncode(recipientPhone)}&text={HttpUtility.UrlEncode(text)}";

            if (!string.IsNullOrEmpty(senderName))
                url += $"&from={senderName}";

            url += $"&digital={digital}&type={type}&answer=json";

            return url;
        }
    }
}