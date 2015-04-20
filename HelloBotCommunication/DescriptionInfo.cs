using System.Collections.Generic;

namespace HelloBotCommunication
{
    public class DescriptionInfo
    {
        /// <summary>
        /// General description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// One string = one sample of using including full command and arguments. Example : "Search y kittens"
        /// </summary>
        public List<string> SamplesOfUsing { get; set; }

        /// <summary>
        /// Scheme of command. Sample : "Search [y|g]  &lt;your query&gt;" where y = Yandex, g = Google (default);
        /// </summary>
        public string CommandScheme { get; set; }

        public DescriptionInfo()
        {
            SamplesOfUsing = new List<string>();
        }
    }
}