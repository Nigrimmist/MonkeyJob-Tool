using System.Collections.Generic;

namespace MonkeyJobTool.Entities.Autocomplete
{
    public class AutocompletePopupInfo
    {
        public List<AutocompletePopupItem> Items { get; set; }

        public AutocompletePopupInfo()
        {
            Items = new List<AutocompletePopupItem>();
        }
    }
}
