using SmartParkingSystem.DataBase.model;
using System;
using SmartParkingSystem.ApplicationLayer.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.Entity;

namespace SmartParkingSystem.BusinessLayer.Repositories
{
    public class Spaces : ISpaces
    {
        private readonly ParkingContext _parkingContext;

        private readonly DbContextOptions<ParkingContext> _options;
        public Spaces(ParkingContext parkingContext, DbContextOptions<ParkingContext> options)
        {
            _parkingContext = parkingContext;
            _options = options;
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
                }).Single(s => s.ParkingNumber == SpaceNumber);

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
            throw new NotImplementedException();
        }

    }
}
