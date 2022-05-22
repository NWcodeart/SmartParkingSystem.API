using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.ApplicationLayer.IRepositories;
using SmartParkingSystem.DataBase.model;
using SmartParkingSystem.Entity;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SmartParkingSystem.BusinessLayer.Repositories
{
    public class Spaces : ISpaces
    {
        private readonly ParkingContext _parkingContext;
        public static IHostingEnvironment _environment;

        private readonly DbContextOptions<ParkingContext> _options;
        public Spaces(
            ParkingContext parkingContext,
            DbContextOptions<ParkingContext> options,
            IHostingEnvironment environment)
        {
            _parkingContext = parkingContext;
            _options = options;
            _environment = environment;
        }

        public int GetIdParkingSpace(string SpaceNumber, int ParkingId)
        {
            using (var db = new ParkingContext(_options))
            {
                var space = db.parkingSpaces.Select(x => new SpacesDto
                {
                    Id = x.Id,
                    ParkingNumber = x.ParkingNumber,
                    ParkingId = x.ParkingId,
                    IsVacant = x.IsVacant,
                    CarNumber = x.CarNumber
                }).Where(s => s.ParkingId == ParkingId).FirstOrDefault(x => x.ParkingNumber == SpaceNumber);

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

        public void InsertCarNumber(string CarNumber, int Id)
            {
            using (var db = new ParkingContext(_options))
            {
                var space = db.parkingSpaces.Single(s => s.Id == Id);
                space.CarNumber = CarNumber;
                space.IsVacant = false;
                db.parkingSpaces.Update(space);
                db.SaveChanges();
            }
        }

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
                using (FileStream filestream = System.IO.File.Create(CurrentDirectory + "\\VPR\\" + files.FileName))
                {
                    files.CopyTo(filestream);
                    filestream.Flush();
                }
            }
        }

        public void CarPlateImageRremove(IFormFile files)
        {
            File.Delete(Environment.CurrentDirectory + "\\VPR\\" + files.FileName);
        }
        public string ExecuteOCR(string imagePath)
        {
            // 1) create process info
            ProcessStartInfo start = new ProcessStartInfo();

            //python cmd full path python.exe
            start.FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python39_64\python.exe";


            // 2) Provide script and arguments
            string arg = imagePath;
            string pathScript = Environment.CurrentDirectory + "\\VPR\\Saudilp.py";
            start.Arguments = $"\"{pathScript}\" \"{arg}\""; //args is path to .py file and any cmd line args

            // 3) process configuration
            start.UseShellExecute = false;
            start.CreateNoWindow = true; //do not create window 
            start.RedirectStandardOutput = true; //recive print lines from the script
            start.RedirectStandardError = true;

            // 4) Execute process and get output
            string result = "";
            string errors = "";

            // take the result printed 
            using (Process process = Process.Start(start))
            {
                errors = process.StandardError.ReadToEnd();
                result = process.StandardOutput.ReadToEnd();
            }

            return result;
        }

        public string rstrip(string text, string RemovedChar = " ")
        {
            string result = "";
            int index = -1;
            if (string.IsNullOrEmpty(text)) { return result; }
            else
            {
                for (int textLength = text.Length - 1; textLength >= 0; textLength--)
                {
                    if (RemovedChar.Contains(text[textLength].ToString()))
                    {
                        index = textLength;
                    }
                    else
                    {
                        if (index >= 0)
                        {
                            result = text.Remove(index);
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public void OCR(IFormFile image, int ParkingId)
        {
            string imagePath = Environment.CurrentDirectory + "\\VPR\\" + image.FileName;
            string CarPlateNumber = ExecuteOCR(imagePath);
            string SpaceNumber = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
            int SpaceId = GetIdParkingSpace(SpaceNumber, ParkingId);
            CarPlateNumber = rstrip(CarPlateNumber, "\r\n");
            InsertCarNumber(CarPlateNumber, SpaceId);
        }

        public string FindCarSpace(string carSpaceName, int parkingId)
        {
            if (string.IsNullOrEmpty(carSpaceName))
            {
                return null;
            }
            else
            {
                using (_parkingContext)
                {
                    try
                    {
                        var SpaceNumber = _parkingContext.parkingSpaces.Where(x => x.ParkingId == parkingId).FirstOrDefault(x => x.CarNumber == carSpaceName);

                        if (SpaceNumber == null)
                        {
                            return null;
                        }
                        else
                        {
                            return SpaceNumber.ParkingNumber;
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                        
                }
            }
        }

        public void unVacantParkingSpace(int Id)
        {
            using (var db = new ParkingContext(_options))
            {
                var space = db.parkingSpaces.Single(s => s.Id == Id);
                space.IsVacant = false;
                db.parkingSpaces.Update(space);
                db.SaveChanges();
            }
        }
    }
}
