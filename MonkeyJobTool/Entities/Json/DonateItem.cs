using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonkeyJobTool.Entities
{
    [Serializable]
    public class DonateItem
    {
        public string Comment { get; set; }
        public double USDCount { get; set; }
        public string From { get; set; }
        public string CreateDate { get; set; }
        public int Id { get; set; }
    }

    public class DonateJson
    {
        public List<DonateItem> Donates { get; set; }

        public DonateJson()
        {
            Donates = new List<DonateItem>();
        }
    }
}
