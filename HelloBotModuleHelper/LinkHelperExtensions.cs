using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace HelloBotModuleHelper
{
    public static class LinkHelperExtensions
    {
        private class idClass
        {
            public string id { get; set; }
        }

        public static string ToShortUrl(this string url)
        {
            string shortenerPostUrl = "https://www.googleapis.com/urlshortener/v1/url";
            string postData = string.Format(@"{{""longUrl"": ""{0}""}}", url);
            HtmlReaderManager hrm = new HtmlReaderManager();
            hrm.ContentType = "application/json";
            hrm.Post(shortenerPostUrl, postData);

            var response = JsonConvert.DeserializeObject<idClass>(hrm.Html);
            return response.id;
        }
    }
}
