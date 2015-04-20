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

        public AuthorInfo(string name)
        {
            Name = name;
        }

        public AuthorInfo(string name, string contactEmail)
        {
            Name = name;
            ContactEmail = contactEmail;
        }
    }
}
