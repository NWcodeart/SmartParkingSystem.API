using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.ApplicationLayer;
using SmartParkingSystem.DataBase.model;
using SmartParkingSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.BusinessLayer
{
    public class Parking : IParking
    {
        private readonly ParkingContext _parkingContext;

        private readonly DbContextOptions<ParkingContext> _options;
        public Parking(ParkingContext parkingContext, DbContextOptions<ParkingContext> options)
        {
            _parkingContext = parkingContext;
            _options = options;
        }

        public void AddCompanyParking(AddParkingDto parking)
        {
            using (_parkingContext)
            {
                var newParking = new CompanyParking()
                {
                    Name = parking.Name
                };

                _parkingContext.companyParkings.Add(newParking);
                _parkingContext.SaveChanges();
            }
        }

        public void AddParkingSpace(AddSpaceDto space)
        {
            using (_parkingContext)
            {
                var newSpace = new ParkingSpace()
                {
                    ParkingNumber = space.ParkingNumber,
                    ParkingId = space.ParkingId
                };

                _parkingContext.parkingSpaces.Add(newSpace);
                _parkingContext.SaveChanges();
            }
        }
            
        public ParkingDto GetCompanyParking(int Id)
        {
            using(var db = new ParkingContext(_options))
            {
                var request = new ParkingDto();

                request = db.companyParkings.Select(x => new ParkingDto
                {
                    Id = x.Id,
                    Name = x.Name,

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
    }
}
