using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SmartParkingSystem.ApplicationLayer;
using SmartParkingSystem.ApplicationLayer.IRepositories;

namespace SmartParkingSystem.BusinessLayer.Repositories
{
    public class Process_StartInfo : IProcess_StartInfo
    {
        public String ExecuteOCR(String imagePath)
        {
            // 1) create process info
            ProcessStartInfo start = new ProcessStartInfo();

            //cmd is full path to python.exe
            start.FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python39_64\python.exe";


            // 2) Provide script and arguments
            string arg = @"D:\graduation project\pythonOCRtest\pythonOCRtest\s10.jpeg";
            string pathScript = @"D:\graduation project\pythonOCRtest\pythonOCRtest\Saudilp.py";
            start.Arguments = $"\"{pathScript}\" \"{arg}\""; //args is path to .py file and any cmd line args

            // 3) process configuration
            start.UseShellExecute = false;
            start.CreateNoWindow = true; //do not create window 
            start.RedirectStandardOutput = true; //recive print lines from the script
            start.RedirectStandardError = true;

            // 4) Execute process and get output
            string result = "";
            string errors = "";

            using (Process process = Process.Start(start))
            {
                errors = process.StandardError.ReadToEnd();
                result = process.StandardOutput.ReadToEnd();
            }

            return result;
        }
    }
}
