﻿using System;
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

namespace Test
{
    
    class Program
    {
        
        static void Main(string[] args)
        {

            Guid commandToken = Guid.NewGuid();
            

            AutoResetEvent autoEvent = new AutoResetEvent(false);
            TimerCallback tcb = state =>
            {
                Test();
            };
            
            Timer stateTimer = new Timer(tcb, autoEvent, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
            while (true)
            {
                autoEvent.WaitOne();
            }


            Console.WriteLine("test");
            Console.ReadLine();

        }
        public static int s= 0;
        public static void Test()
        {
            Console.WriteLine(s+" "+DateTime.Now);
            Thread.Sleep(10000);
            Console.WriteLine(s++ + " " + DateTime.Now);
        }
    }

}
