using SmartParkingSystem.DataBase.model;
using System;
using SmartParkingSystem.ApplicationLayer.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.Entity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Python.Runtime;
using System.Diagnostics;

namespace SmartParkingSystem.BusinessLayer.Repositories
{
    public class Spaces : ISpaces
    {
        private readonly ParkingContext _parkingContext;
        public static IHostingEnvironment _environment;
        PyScope _scope; 

        private readonly DbContextOptions<ParkingContext> _options;
        public Spaces(ParkingContext parkingContext, DbContextOptions<ParkingContext> options, IHostingEnvironment environment)
        {
            _parkingContext = parkingContext;
            _options = options;
            _environment = environment;
        }

        public int GetIdParkingSpace(string SpaceNumber)
        {
            using (var db = new ParkingContext(_options))
            {
                var space = new SpacesDto();

                space = db.parkingSpaces.Select(x => new SpacesDto
                {
                    Id = x.Id,
                    ParkingNumber = x.ParkingNumber,
                    ParkingId = x.ParkingId,
                    IsVacant = x.IsVacant,
                    CarNumber = x.CarNumber
                }).SingleOrDefault(s => s.ParkingNumber == SpaceNumber);

                if (space != null)
                {
                    return space.Id;
                }
                else
                {
                    return 0;
                };
            }
        }

        public SpacesDto GetParkingSpace(int Id)
        {
            using (var db = new ParkingContext(_options))
            {
                var request = new SpacesDto();

                request = db.parkingSpaces.Select(x => new SpacesDto
                {
                    Id = x.Id,
                    ParkingNumber = x.ParkingNumber,
                    ParkingId = x.ParkingId,
                    IsVacant = x.IsVacant,
                    CarNumber = x.CarNumber
                }).Single(p => p.Id == Id);

                if (request != null)
                {
                    return request;
                }
                else
                {
                    return null;
                }
            }
        }

        //public void InsertCarNumber(string CarNumber, int Id)
        //{
        //    using (var db = new ParkingContext(_options))
        //    {
        //        var space = db.parkingSpaces.Single(s => s.Id == Id);
        //        space.CarNumber = CarNumber;
        //        space.IsVacant = false;
        //        db.parkingSpaces.Update(space);
        //        db.SaveChanges();
        //    }
        //}

        public void VacantParkingSpace(int Id)
        {
            using (var db = new ParkingContext(_options))
            {
                var space = db.parkingSpaces.Single(s => s.Id == Id);
                space.CarNumber = null;
                space.IsVacant = true;
                db.parkingSpaces.Update(space);
                db.SaveChanges();
            }
        }

        public void DeleteSpaceParking(int Id)
        {
            var Space = _parkingContext.parkingSpaces.Where(x => x.Id == Id).FirstOrDefault();

            if (Space != null)
            {
                _parkingContext.parkingSpaces.Remove(Space);
                _parkingContext.SaveChanges();
            }
        }

        public void CarPlateImageStore(IFormFile files)
        {
            var CurrentDirectory = Environment.CurrentDirectory;
            if (files.Length > 0)
            {
                using (FileStream filestream = System.IO.File.Create(CurrentDirectory + "\\OCR\\" + files.FileName))
                {
                    files.CopyTo(filestream);
                    filestream.Flush();
                }
            }
        }

        public void CarPlateImageRremove(IFormFile files)
        {
            File.Delete(Environment.CurrentDirectory + "\\OCR\\" + files.FileName);
        }

        public void OCR(IFormFile files )
        {
            PythonEngine.Initialize();
            _scope = Py.CreateScope();

            using (Py.GIL()) //Initialize the Python engine and acquire the interpreter lock
            {
                try
                {
                    // import your script into the process
                    dynamic cv2 = Py.Import("cv2");
                    dynamic numpy = Py.Import("numpy");
                    dynamic imutils = Py.Import("imutils");
                    dynamic pytesseract = Py.Import("pytesseract");
                    dynamic easyocr = Py.Import("easyocr");

                    _scope.Exec(File.ReadAllText(Environment.CurrentDirectory + "\\OCR\\saudilp.py"));
                    //PyObject p = PythonEngine.Compile("", Environment.CurrentDirectory + "\\Archive\\saudilp.py", RunFlagType.File);

                }
                catch (PythonException error)
                {
                    Console.WriteLine("Error occured: ", error.Message);
                }

            }


            // 1) create process info 
            ProcessStartInfo start = new ProcessStartInfo();

            //cmd is full path to python.exe
            start.FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python39_64\python.exe";


            // 2) Provide script and arguments
            string arg = Environment.CurrentDirectory + "\\OCR\\" + files.FileName;
            string pathScript = Environment.CurrentDirectory + "\\OCR\\saudilp.py";
            start.Arguments = $"\"{pathScript}\"\"{arg}\""; //args is path to .py file and any cmd line args

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
                result = process.StandardOutput.ReadToEnd();
                errors = process.StandardError.ReadToEnd(); 
            }


        }
    }
}
