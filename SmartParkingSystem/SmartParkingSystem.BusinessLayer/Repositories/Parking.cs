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
                    ParkingId = space.ParkingId,
                    IsVacant = space.IsVacant
                };

                _parkingContext.parkingSpaces.Add(newSpace);
                _parkingContext.SaveChanges();
            }
        }

        public void DeleteCompanyParking(int Id)
        {
            var parking = _parkingContext.companyParkings.Where(x => x.Id == Id).FirstOrDefault();

            if(parking != null)
            {
                _parkingContext.companyParkings.Remove(parking);
                _parkingContext.SaveChanges();
            }
        }

        public List<ParkingDto> GetAllCompanyParking()
        {
            using (_parkingContext)
            {
                var ParkingList = new List<ParkingDto>();
                ParkingList = _parkingContext.companyParkings.Select(x => new ParkingDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParkingList = x.ParkingList.Select( p => new SpacesDto
                    {
                        Id = p.Id,
                        ParkingNumber = p.ParkingNumber,
                        ParkingId = p.ParkingId,
                        IsVacant = p.IsVacant,
                        CarNumber = p.CarNumber
                    }).ToList()
                }).ToList();

                if(ParkingList == null)
                {
                    return null;
                }
                else
                {
                    return ParkingList;
                }
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
                    ParkingList = x.ParkingList.Select(p => new SpacesDto
                    {
                        Id = p.Id,
                        ParkingNumber = p.ParkingNumber,
                        ParkingId = p.ParkingId,
                        IsVacant = p.IsVacant,
                        CarNumber = p.CarNumber
                    }).ToList()
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

    }
}
