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

namespace SmartParkingSystem.BusinessLayer.Repositories
{
    public class Spaces : ISpaces
    {
        private readonly ParkingContext _parkingContext;
        public static IHostingEnvironment _environment;

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
                using (FileStream filestream = System.IO.File.Create(CurrentDirectory + "\\CarPlateImage\\" + files.FileName))
                {
                    files.CopyTo(filestream);
                    filestream.Flush();
                }
            }
        }

        public void CarPlateImageRremove(IFormFile files)
        {
            File.Delete(Environment.CurrentDirectory + "\\CarPlateImage\\" + files.FileName);
        }

        public void OCR(IFormFile files )
        {
            var engine = Python.CreateEngine();
            var scope =  engine.CreateScope();

            var CurrentDirectory = Environment.CurrentDirectory;

            ScriptSource source = engine.CreateScriptSourceFromFile(CurrentDirectory + "\\OCR\\saudilp.py");
            object result = source.Execute(scope);
        }
    }
}
