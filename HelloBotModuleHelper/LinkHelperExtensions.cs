using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;


namespace HelloBotModuleHelper
{
    public static class LinkHelperExtensions
    {
       
        public static string ToShortUrl(this string url)
        {
            string shortenerPostUrl = string.Format("http://tinyurl.com/create.php?source=indexpage&url={0}&submit=Make+TinyURL%21&alias=", Uri.EscapeUriString(url));
           
            HtmlReaderManager hrm = new HtmlReaderManager();
            hrm.Post(shortenerPostUrl,"");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(hrm.Html);
            return doc.DocumentNode.SelectSingleNode("//div[@class='indent']/b").InnerText;
        }
    }
}
