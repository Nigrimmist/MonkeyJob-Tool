using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonkeyJobTool.Controls.Settings
{
    public interface IUIDataItem
    {
        string PropName { get; set; }
        int DeepLevel { get; set; }
        int? CollectionIndex { get; set; }
    }
}
