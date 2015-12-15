using System;
using System.Collections.Generic;

namespace HelloBotCommunication
{
    public class SuggestionDetails
    {
        public string Key { get; set; }
        public Func<string,List<AutoSuggestItem>> GetSuggestionFunc { get; set; }
    }
}