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

        //add new parking for a building or a company
        public void AddParkingSpace(AddSpaceDto space);

        //get Id of space parking by name of space
        public int GetIdParkingSpace(String SpaceNumber);

        //get space parking Id by parking 
        public SpacesDto GetParkingSpace(int Id);

        //this Function will insert the car plate number to the parking space and update space state to unavailable
        public void InsertCarNumber(string CarNumber, int Id);

        //update state of parking to available and delete car number
        public void VacantParkingSpace(int Id);
    }
}
