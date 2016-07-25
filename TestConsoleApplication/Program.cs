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

            var t = new test();
            t.gg = new List<string>();
            t.gg.Add("");

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
        private static string GetAuthorizationHeader()
        {
            AuthenticationResult result = null;

            var context = new AuthenticationContext("https://login.microsoftonline.com/91c6f46c-de8c-46f7-bbd7-67f8b9be58c6/oauth2/token");

            var thread = new Thread(() =>
            {
                result = context.AcquireTokenAsync("https://management.core.windows.net/", "85a6eb44-8ef4-49a0-aa68-fbc07fb45f91", new Uri("http://localhost"),new PlatformParameters(PromptBehavior.Auto)).Result;
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = "AquireTokenThread";
            thread.Start();
            thread.Join();

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }

            string token = result.AccessToken;
            var s = result.CreateAuthorizationHeader();
            
            return token;
        }

        private static string GetAuthorizationHeader2()
        {
            AuthenticationResult result = null;

            var context = new AuthenticationContext("https://login.microsoftonline.com/91c6f46c-de8c-46f7-bbd7-67f8b9be58c6/oauth2/authorize");

            var thread = new Thread(() =>
            {
                result = context.AcquireTokenAsync("https://management.core.windows.net/", "85a6eb44-8ef4-49a0-aa68-fbc07fb45f91", new Uri("http://localhost"), new PlatformParameters(PromptBehavior.Auto)).Result;
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = "AquireTokenThread";
            thread.Start();
            thread.Join();

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }

            string token = result.AccessToken;
            var s = result.CreateAuthorizationHeader();

            return token;
        }

        public static bool SetFirewallRuleAutoDetect(string certFilename, string certPassword, string subscriptionId, string serverName, string ruleName)
        {

            try
            {
                string url = string.Format("https://management.database.windows.net:8443/{0}/servers/{1}/firewallrules/{2}?op=AutoDetectClientIP",
                                           subscriptionId,
                                           serverName,
                                           ruleName);

                HttpWebRequest webRequest = HttpWebRequest.Create(url) as HttpWebRequest;

                
                webRequest.Method = "POST";
                webRequest.Headers["x-ms-version"] = "1.0";
                webRequest.ContentLength = 0;

                // call the management api
                // there is no information contained in the response, it only needs to work
                using (WebResponse response = webRequest.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader sr = new StreamReader(stream))
                {
                    Console.WriteLine(sr.ReadToEnd());
                }

                // the firewall was successfully updated
                return true;
            }
            catch
            {
                // there was an error and the firewall possibly not updated
                return false;
            }
        }

        
    }

}
