using System;
using System.Collections.Generic;

namespace HelloBotCommunication
{
    public class SuggestionDetails
    {
        public string Key { get; set; }
        public Func<List<AutoSuggestItem>> GetSuggestionFunc { get; set; }
    }
}