using Microsoft.AspNetCore.Http;
using SmartParkingSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.ApplicationLayer.IRepositories
{
    public interface ISpaces
    {
        //get Id of space parking by name of space
        public int GetIdParkingSpace(String SpaceNumber);

        //get space parking Id by parking 
        public SpacesDto GetParkingSpace(int Id);

        //Delete Space parking by Id
        public void DeleteSpaceParking(int Id);

        //this Function will insert the car plate number to the parking space and update space state to unavailable
        //public void InsertCarNumber(string CarNumber, int Id);

        //update state of parking to available and delete car number
        public void VacantParkingSpace(int Id);

        //store car plate image in CarPlateImage folder 
        public void CarPlateImageStore(IFormFile files);

        //remove car plate image from CarPlateImage folder 
        public void CarPlateImageRremove(IFormFile files);

        public void OCR(IFormFile files);
    }
}
