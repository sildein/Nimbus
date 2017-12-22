/*
 * Program.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Nimbus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            foreach (string Option in File.ReadLines("config.ini"))
            {
                string[] SplitLine = Option.Split('=');
                switch (SplitLine[0])
                {
                    case "Title":
                        Shared.Title = SplitLine[1];
                        break;

                    case "Prefix":
                        Shared.Prefix = SplitLine[1];
                        break;

                    case "Port":
                        Shared.Port = SplitLine[1];
                        break;

                    default:
                        break;
                }
            }

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls(String.Format("http://0.0.0.0:{0}/", Shared.Port))
                .Build();
    }
}
