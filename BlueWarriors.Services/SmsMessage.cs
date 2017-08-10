using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BlueWarriors.Services
{
    public class SmsMessage : ISmsMessage
    {
        public void Send(string message, string recipient)
        {
            string formattedMessage = message.Replace(" ", "%20");
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://172.18.0.2/");
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("text/html");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync($"sms_sender.php?MSISDN={recipient}&MESSAGE={formattedMessage}").Result;
                string data = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrWhiteSpace(data))
                {
                    throw new Exception($"Failed to send SMS message to {recipient}");
                }
            }
        }
    }
}