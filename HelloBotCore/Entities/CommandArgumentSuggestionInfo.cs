using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public class CommandArgumentSuggestionInfo : ArgumentSuggestionInfo
    {
        public List<RegexParseitem> TemplateParseInfo { get; set; }

        public CommandArgumentSuggestionInfo(ArgumentSuggestionInfo item)
        {
            this.Details = item.Details;
            this.ArgTemplate = item.ArgTemplate;
            TemplateParseInfo = new List<RegexParseitem>();

            string userReg = item.ArgTemplate;
            Regex r = new Regex(@"\{\{.*?\}\}");
            List<string> delimeters = (from Match match in r.Matches(item.ArgTemplate) select match.ToString()).ToList();
            for (var i = 0; i < delimeters.Count; i++)
            {
                var regPart = userReg.Substring(0, userReg.IndexOf(delimeters[i]));
                TemplateParseInfo.Add(new RegexParseitem()
                {
                    Key = delimeters[i].Replace("{{", "").Replace("}}", ""),
                    RegexpPart = regPart,
                    Order = i
                });
                userReg = userReg.Substring(regPart.Length + delimeters[i].Length);
            }
        }
    }
}
