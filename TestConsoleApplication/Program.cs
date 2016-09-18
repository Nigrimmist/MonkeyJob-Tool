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

                    



                    //Console.WriteLine(sb.ToString());
                    
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
    }

}
