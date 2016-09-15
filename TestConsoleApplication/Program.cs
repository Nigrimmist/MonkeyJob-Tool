using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using System.Web;
using System.Xml;
using HelloBotCore;
using HtmlAgilityPack;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MonkeyJobTool.Extensions;
using Newtonsoft.Json;
using NetFwTypeLib;
namespace Test
{
    public class test
    {
        private List<string> _gg;

        public List<string> gg
        {
            get
            {
                return _gg;
            }
            set
            {
                _gg = value;
            }
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {

            HtmlReaderManager hrm = new HtmlReaderManager();

            Regex r = new Regex("[а-яА-ЯЁё]+");
            //bool isRu = r.IsMatch(args);
            //string fromLang = isRu ? "ru" : "en";
            //string toLang = isRu ? "en" : "ru";

            
            //hrm.SendReferer = "http://www.translate.ru/";
            try
            {
                while (true)
                {
                    Console.WriteLine("слово :");
                    var s = Console.ReadLine();

                    hrm.Get("http://www.translate.ru/");
                    hrm.ContentType = "application/json; charset=utf-8";
                    hrm.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    hrm.Post("http://www.translate.ru/services/TranslationService.asmx/GetTranslateNew",
                    string.Format("{{dirCode:'en-ru', template:'General', text:'{0}', lang:'en', limit:3000,useAutoDetect:true, key:'', ts:'MainSite',tid:'',IsMobile:false}}", HttpUtility.UrlEncode(s)));
                    string decoded = DecodeEncodedNonAsciiCharacters(hrm.Html);
                    decoded = decoded.Replace(@"\""", @"""");
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(decoded);
                    var variantsNodes = doc.DocumentNode.SelectNodes("//div[@class='cforms_result']");
                    foreach (var node in variantsNodes)
                    {
                        var title = node.SelectSingleNode("//span[@class='source_only']").InnerText;

                    }
                    
                    //Console.WriteLine(string.Join("\r\n", variants));
                    
                }
                
                

                

                

                

                
                
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse resp = e.Response;
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        Console.WriteLine(sr.ReadToEnd());
                    }
                }
            }
            

            //var token = GetAuthorizationHeader2();
            ////
            
            ////https://management.core.windows.net:8443/{subscriptionId}/services/sqlservers/servers/Contoso/firewallrules

            //string url = string.Format("https://management.core.windows.net:8443/{0}/services/sqlservers/servers/{1}/firewallrules",
            //                               "b67bb366-fcc0-497f-b027-de1c33d507ba",
            //                               "wtmgqggt8p",
            //                               "test1");


            


            //HtmlReaderManager hrm = new HtmlReaderManager();
            //hrm.Headers.Add("Authorization", "Bearer " + token);
            //hrm.Headers["x-ms-version"] = "2012-03-01";
            //var json = JsonConvert.SerializeObject(new {ServiceResource = new {Name = "test1",StartIPAddress="134.17.155.201",EndIPAddress="134.17.155.210"}});
            //hrm.Post(url, json);
            //string s = hrm.Html;
            ////clientid : 85a6eb44-8ef4-49a0-aa68-fbc07fb45f91
            ////tenant : 91c6f46c-de8c-46f7-bbd7-67f8b9be58c6

            ////SetFirewallRuleAutoDetect("Nigrimmist@gmail.com", "5716188xf3z54dlc", "b67bb366-fcc0-497f-b027-de1c33d507ba", "tce6f5yqv6", "test1");
            
            Console.WriteLine("test");
            Console.ReadLine();

        }



        

        static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m =>
                {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }

        
    }

}
