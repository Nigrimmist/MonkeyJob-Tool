using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCommunication
{
    public class AuthorInfo
    {
        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public string EmailForLogs { get; set; }
        
        /// <summary>
        /// Author information for user/system
        /// </summary>
        /// <param name="name">Your name or nickname</param>
        /// <param name="contactEmail">Your contact email. It will be visible to all users</param>
        /// <param name="emailForErrorLogs">Email for error reports, every single error will be send from MonkeyJobSender@gmail.com service email. Use it, Luke!</param>
        public AuthorInfo(string name, string contactEmail, string emailForErrorLogs)
        {
            Name = name;
            ContactEmail = contactEmail;
            EmailForLogs = emailForErrorLogs;
        }
    }
}
