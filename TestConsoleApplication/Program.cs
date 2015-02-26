using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

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
    class Program
    {
        private static Random r = new Random();
        public static List<string> Jokes = new List<string>(); 
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            string str = "фшдывофдоырвофрыодлвфолвлфывлфвоырвыфовфырвллфорвлоырвлофырв";
            for (var i = 0; i <= 20; i++)
            {
                str += str;
            }
            var ss =str.GetOtherKeyboardLayoutWords()[0];
           
            Stopwatch sw = new Stopwatch();
           
           
            sw.Start();
            string s2 = str.GetOtherKeyboardLayoutWords()[0];
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            
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

        static string Switch(string s)
        {
            foreach (var p in "йцукенгшщзхъфывапролджэячсмитьбю.ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,".Zip("qwertyuiop[]asdfghjkl;'zxcvbnm,./QWERTTYYIOOOP{}ASDFGHJKL:\"ZXCVBNM<>?",
             (oldC, newC) => new { oldC, newC }))
                s = s.Replace(p.oldC, p.newC);
            return s;
            
        }
    }
}
