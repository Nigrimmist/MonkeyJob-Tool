﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace Yushko.Modules
{
    class SlugPost {
        public SlugPost() {}
        public String slug { get; set; }
        public String text { get; set; }
        public String html { get; set; }
        public int viewsCount { get; set; }
        public bool isApproved { get; set; }
        public bool isInVk { get; set; }
        public bool result { get; set; }
        public String title { get; set; }
        public String socialTitle { get; set; }
    }

     class Slug {
        public String indexSlug { get; set; }
        public SlugPost post;
        public String nextSlug { get; set; }
    }

    public class Sorry : ModuleHandlerBase
    {
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
        }
        public override double ModuleVersion
        {
            get { return 1.0; }
        }

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("простите.com", new List<string>(){"prostite","простите"} ),
                });
            }
        }
        
        private string NextSlug = String.Empty;

        /// <summary>
        /// Sending GET request.
        /// </summary>
        /// <param name="Url">Request Url.</param>
        /// <param name="Headers">Request Headers.</param>
        /// <param name="Data">Data for request.</param>
        /// <returns>Response body.</returns>
        public static string HTTP_GET(string Url, NameValueCollection Headers,string Data)
        {
            string result = String.Empty;
            WebRequest req = WebRequest.Create(Url + (string.IsNullOrEmpty(Data) ? "" : "?" + Data));
            req.Headers.Add(Headers);
            try
            {
                WebResponse resp = req.GetResponse();
                using (Stream stream = resp.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                result = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                result = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                result = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return result;
        }

        public override string ModuleDescription { get { return @"казнить нельзя помиловать"; } }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            string result = String.Empty;
            NameValueCollection headers = new NameValueCollection();
            headers.Add("X-Requested-With", "XMLHttpRequest");
            string url = "https://prostite.com";
            string resp = HTTP_GET(url + "/" + NextSlug, headers, "");
            result = resp;
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                Slug slugobj = json_serializer.Deserialize<Slug>(resp);
                NextSlug = slugobj.nextSlug;
                result = slugobj.post.text;
            }
            finally
            {
                _client.ShowMessage(commandToken,result);
            }
        }
    }
}
