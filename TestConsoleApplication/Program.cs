using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using System.Web;
using System.Xml;
using HelloBotCore;

using MonkeyJobTool.Extensions;
using Nigrimmist.Modules.Commands;

namespace Test
{
    public class t
    {
        public string n { get; set; }
        public List<a> a { get; set; }

        public t()
        {
            a = new List<a>() {new a() {sss = "sss"}};
        }
    }

    public class a
    {
        public string sss { get; set; }
    }
    class Program
    {
        
        static void Main(string[] args)
        {

            List<t> s = new List<t>();
            s.Add(new t(){n = "sss"});

            foreach (t t in s)
            {
                t.n = "lol";
                t.a.First().sss = "changed";
            }
            //Console.WriteLine(s);
            //Console.WriteLine(s2);
            //try
            //{
            //    HelloBot bot = new HelloBot();
            //    bot.HandleMessage("!calc 1+2", (s, type) => { Console.WriteLine(s); }, null);
            //}
            //catch (Exception ex)
            //{

            //    Console.WriteLine(ex.ToString());
            //}

            
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
