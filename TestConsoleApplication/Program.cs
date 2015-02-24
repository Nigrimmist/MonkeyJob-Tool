using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using System.Web;
using System.Xml;
using HelloBotCore;
using Nigrimmist.Modules.Commands;

namespace Test
{
    class Program
    {
        private static Random r = new Random();
        public static List<string> Jokes = new List<string>(); 
        static void Main(string[] args)
        {
            try
            {
                HelloBot bot = new HelloBot();
                bot.HandleMessage("!calc 1+2", (s, type) => { Console.WriteLine(s); }, null);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
            
            

            //List<string> s2 = new List<string>() { "Сиськи" };

            //foreach (var v in s2)
            //{
            //    new Quote().HandleMessage(v, null, s =>
            //        Console.WriteLine(s));
            //}
            Console.WriteLine("test");
            Console.ReadLine();
        }
    }
}
