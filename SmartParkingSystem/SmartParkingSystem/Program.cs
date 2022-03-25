using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParkingSystem
{
    public class Program
    {
        private static string pythonPath1 = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python39_64";

        public static void Main(string[] args)
        {
            //Test();
            CreateHostBuilder(args).Build().Run();
        }
        private static void Test()
        {
            string pathToPython = pythonPath1;
            string path = pathToPython + ";" + Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToPython, EnvironmentVariableTarget.Process);

            var lib = new[]
            {
            @"D:\ÇáãÓÊäÏÇÊ\graduation project\smart parking system\SmartParkingSystem\SmartParkingSystem\Archive\saudilp.py",
            Path.Combine(pathToPython, "Lib"),
            Path.Combine(pathToPython, "DLLs")

        };
            string paths = string.Join("; ", lib);
            Environment.SetEnvironmentVariable("PYTHONPATH", paths, EnvironmentVariableTarget.Process);

        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
