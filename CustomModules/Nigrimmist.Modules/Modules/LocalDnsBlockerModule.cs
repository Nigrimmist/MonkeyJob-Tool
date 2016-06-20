using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;

namespace Nigrimmist.Modules.Modules
{

    public class LocalDnsBlockerModule : ModuleCommandBase
    {
        private IClient _client;
        private string hostsPath = string.Empty;
        public override void Init(IClient client)
        {
            _client = client;
            hostsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts");
        }
        public override double Version
        {
            get { return 1.0; }
        }

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("IP Фаервол", new List<string>() {"dnson","dnsoff","firewall","фаервол"}, new ReadOnlyCollection<ArgumentSuggestionInfo>(new List<ArgumentSuggestionInfo>()
                    {
                        new ArgumentSuggestionInfo()
                        {
                            ArgTemplate = @"{{dnsName}}",
                            Details = new List<SuggestionDetails>()
                            {
                                new SuggestionDetails() {Key = "dnsName", GetSuggestionFunc = (text) => _client.GetSettings<LocalDnsBlockerSettings>().DnsList.Where(x=>x.Title.Contains(text)).Select(x=>new AutoSuggestItem(){Value = x.Title,DisplayedKey = x.Title}).ToList()}
                            }
                        }
                    })),
                });
            }
        }


        public override string Title
        {
            get { return "IP Фаервол"; }
        }

        

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "",
                    CommandScheme = "курс <кол-во> <код_валюты_из> <код_валюты_в>",
                    SamplesOfUsing = new List<string>()
                    {
                        "курс 1 usd byr",
                        "курс ставкареф",
                        "курс коды",
                        "курс все",
                        "курс help"
                    }

                };
            }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAB1UlEQVRIid2VIUiDURSFv7CwsGAwGAyGCUaD4oKCgjCDgojBYBCTwbBoEAwiIoYFbSILSyqITUREZBZBsQgiC4YJIgaDiMLCb3jv8d9d735m1AsPtnvuOee9t3vf4L9FJ1ABcr/gjALHwDLQm1SYAW6BCHgGsi0aTHpOWDVgBxgH0rKwrArnWjRoUzy5dkLRFPAlgGug3xDr80vHpSF+BUyEgnMFThsiWYHr69syDI4COKCAbUN8BngTNW+4Ow4xbxjUgTzAggLalXgH8O6xKvAqPqd8TZfP3SutdYCSSFwYuw9d8o7rijSuyyIa23LMbyZ0YoRrX6oiUTQMhgW+4E/YJnavoyjqHwHuRGLDIKRwwyePnjSMBVH3AvAgEqtNSBncpMrjf+LuPsngCRp/mJJBaMcN3Zj/Poyb1sib6lhBXdGBSmS8SIgl4q4Jcepzm4bBntA71I4R8TX0eIIcsCquM8L3USWeJm7pyG+OEeBDmUTAoiAWFFbH7rg1VZcPwL5hsKvIuYSdB7yuTpsCGAJODIMbQ6QPGDTy3cCZ4s8HsMRP8bC6DDErZhWvIsGMLygTvzNhzbRokCV+Pmq4J8OMFO4KVnHdZD3bzSLnOYl/m38vvgExuNOCw1vhrQAAAABJRU5ErkJggg=="; }
        }

        
        //todo : refactoring required
        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            string result = "";
            if (args.EndsWith("-"))
            {
                string dnsTitle = args.TrimEnd(" -".ToCharArray());
                var dns = _client.GetSettings<LocalDnsBlockerSettings>().DnsList.Where(x => x.Title == dnsTitle).Select(x => x.DnsName).SingleOrDefault();
                if (!string.IsNullOrEmpty(dns))
                {
                    DnsBlockOn(dns);
                    _client.ShowMessage(commandToken, "Доступ к " + dnsTitle + " заблокирован");
                }
            }
            if (args.EndsWith("+"))
            {
                string dnsTitle = args.TrimEnd(" +".ToCharArray());
                TimeSpan? timeout = null;
                var spl = dnsTitle.Split(' ');
                int minCount;
                if (int.TryParse(spl.Last(), out minCount))
                {
                    timeout = TimeSpan.FromMinutes(minCount);
                    dnsTitle = string.Join("", spl.Take(spl.Length - 1));
                }
                var dns = _client.GetSettings<LocalDnsBlockerSettings>().DnsList.Where(x => x.Title == dnsTitle).Select(x => x.DnsName).SingleOrDefault();
                if (!string.IsNullOrEmpty(dns))
                {
                    DnsBlockOff(dns);
                    string msg = "Доступ к " + dnsTitle + " предоставлен";
                    if (timeout.HasValue)
                    {
                        msg += "на " + timeout.Value.TotalMinutes + " мин.";
                        Task.Factory.StartNew(delegate
                        {
                            Thread.Sleep((int)timeout.Value.TotalMilliseconds);
                            DnsBlockOn(dns);
                            _client.ShowMessage(commandToken, "Доступ к " + dnsTitle + " заблокирован");
                        });
                    }
                    _client.ShowMessage(commandToken, msg);

                }
            }
        }

        private void DnsBlockOff(string dnsName)
        {
            var lines = File.ReadLines(hostsPath).ToList();
            string found = lines.FirstOrDefault(x => x.Contains(dnsName));
            if (!string.IsNullOrEmpty(found))
            {
                lines.Remove(found);
                File.WriteAllLines(hostsPath, lines);
            }
        }

        private void DnsBlockOn(string dnsName)
        {
            var lines = File.ReadLines(hostsPath).ToList();
            bool found = lines.Any(x=>x.Contains(dnsName));
            if (!found)
            {
                lines.Add("127.0.0.2"+" "+dnsName);
                File.WriteAllLines(hostsPath, lines);
            }
        }

        
    }

    [ModuleSettingsFor(typeof(LocalDnsBlockerModule))]
    public class LocalDnsBlockerSettings
    {
        [SettingsNameField("Днсы : ")]
        public List<LocalDnsBlockerItemSettings> DnsList { get; set; }

        public LocalDnsBlockerSettings()
        {
            DnsList = new List<LocalDnsBlockerItemSettings>();
        }
    }

    public class LocalDnsBlockerItemSettings
    {
        [SettingsNameField("Днс")]
        public string DnsName { get; set; }

        [SettingsNameField("Title")]
        public string Title { get; set; }

        public LocalDnsBlockerItemSettings()
        {
            
        }
    }
}
