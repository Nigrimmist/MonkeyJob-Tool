using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCommunication
{
    public class UniqueMessageHash
    {
        /// <summary>
        /// Your unique string id
        /// </summary>
        public string UniqueString { get; set; }

        /// <summary>
        /// Fill it if you would like to separate unique messages by different groups
        /// </summary>
        public string GroupId { get; set; }



        /// <param name="uniqueString">Your unique string id</param>
        public UniqueMessageHash(string uniqueString)
        {
            UniqueString = uniqueString;
            GroupId = string.Empty;
        }


        /// <param name="uniqueString">Your unique string id</param>
        /// <param name="groupId">Fill it if you would like to separate unique messages by different groups</param>
        public UniqueMessageHash(string uniqueString, string groupId)
        {
            UniqueString = uniqueString;
            GroupId = groupId;
        }
    }
}
