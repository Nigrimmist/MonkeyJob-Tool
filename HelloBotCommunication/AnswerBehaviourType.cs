using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HelloBotCommunication
{
    public enum AnswerBehaviourType
    {
        /// <summary>
        /// Simply show text
        /// </summary>
        ShowText,
        /// <summary>
        /// Will open your link in browser. Should start with http:// or https://
        /// </summary>
        OpenLink,
        /// <summary>
        /// Will copy provided string to clipboard
        /// </summary>
        CopyToClipBoard
    }
}
