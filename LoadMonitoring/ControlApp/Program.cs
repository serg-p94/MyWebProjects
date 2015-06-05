﻿using System;
using System.Threading;

namespace ControlApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var mm = Bootstrapper.Bootstrapper.GetMemoryMonitor();
            var hm = Bootstrapper.Bootstrapper.GetHddMonitor();

            Console.WriteLine("Starting monitoring...");
            mm.Start();
            hm.Start();

            Thread.Sleep(10000);

            mm.Stop();
            hm.Stop();

            Console.WriteLine("Monitoring finished!");
        }
    }
}