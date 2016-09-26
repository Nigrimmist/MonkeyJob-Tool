using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MonkeyJobTool.Managers;

namespace MonkeyJobTool.Helpers
{
    public class GoogleAnalytics
    {
        private const string GATrackId = "UA-38211218-5";
        private static Random _rnd = new Random();

        public static void LogOpenDonateListForm()
        {
            GACall(GATrackId, "donate_list_open");
        }

        public static void LogDonateLinkClicked()
        {
            GACall(GATrackId, "donate_click");
        }

        public static void LogOpenDonateForm()
        {
            GACall(GATrackId, "donate_open");
        }

        public static void LogFirstUse()
        {
            GACall(GATrackId, "first_run");
        }

        public static void LogRun()
        {
            GACall(GATrackId, "run");
        }

        private static void GACall(string trackingId, string campaign)
        {
            LogManager.Trace("Start GACall " + trackingId + " " + campaign);

            string culture = Thread.CurrentThread.CurrentCulture.Name;
            string screenRes = Screen.PrimaryScreen.Bounds.Width + "x" + Screen.PrimaryScreen.Bounds.Height;
            string pageName = Uri.EscapeUriString("log");
            string os = GetOSFriendlyName();
            string utmcc = string.Format("__utma=234084878.479851276.1333418536.1333418536.1333418536.1;+__utmz=234084878.1333418536.1.1.utmcsr=({0})|utmccn=(direct)|utmcmd=({1});", Uri.EscapeUriString(campaign), Uri.EscapeUriString(os));

            string statsRequest = "http://www.google-analytics.com/__utm.gif" +
                                  "?utmwv=4.6.5" +
                                  "&utmn=" + _rnd.Next(100000000, 999999999) +
                                  "&utmcs=-" +
                                  "&utmsr=" + screenRes +
                                  "&utmsc=-" +
                                  "&utmul=" + culture +
                                  "&utmje=-" +
                                  "&utmfl=-" +
                                  "&utmdt=" + pageName +
                                  "&utmr=0" +
                                  "&utmp=" + pageName +
                                  "&utmac=" + trackingId +
                                  "&utmcc=" +
                                  Uri.EscapeUriString(utmcc);


            using (var client = new WebClient())
            {
                client.DownloadData(statsRequest);
            }
            LogManager.Trace("End GACall");
        }

        public static string GetOSFriendlyName()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }
    }
}
