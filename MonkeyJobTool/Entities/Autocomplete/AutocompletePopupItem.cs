using System.Collections.Generic;

namespace MonkeyJobTool.Entities.Autocomplete
{
    public class AutocompletePopupItem
    {
        public List<SelectableWordPart> WordParts { get; set; } 
        public string Value { get; set; }
    }
}
