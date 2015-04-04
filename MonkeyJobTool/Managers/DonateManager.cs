using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Helpers;
using Newtonsoft.Json;

namespace MonkeyJobTool.Managers
{
    class DonateManager
    {
        public static List<DonateItem> GetDonateList()
        {
            HtmlReaderManager h = new HtmlReaderManager();
            h.Get(AppConstants.Urls.DonateListUrl);
            string html = h.Html;

            var retList = JsonConvert.DeserializeObject<DonateJson>(html).Donates;
            retList.Sort(delegate(DonateItem n1, DonateItem n2)
            {
                if (n1.USDCount < n2.USDCount)
                {
                    return 1;
                }

                if (n1.USDCount > n2.USDCount)
                {
                    return -1;
                }

                return 0;
            });

            return retList;
        }
    }
}
