using SmartParkingSystem.DataBase.model;
using SmartParkingSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.ApplicationLayer
{
    public interface IParking
    {
        //add new parking for a building or a company
        public void AddCompanyParking(AddParkingDto parking);

        //get company parking by Id
        public ParkingDto GetCompanyParking(int Id);

        //get all company parking
        public List<ParkingDto> GetAllCompanyParking();

        //delet company parking by Id
        public void DeleteCompanyParking(int Id);

        //add new parking for a building or a company
        public void AddParkingSpace(AddSpaceDto space);

    }
}
