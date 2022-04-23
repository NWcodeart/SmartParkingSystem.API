using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingSystem.ApplicationLayer.IRepositories;
using System;
using Python.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParkingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpacesController : ControllerBase
    {
        private ISpaces _spaces;
        public SpacesController(ISpaces spacesRepo)
        {
            _spaces = spacesRepo;
        }
        [HttpGet]
        [Route("GetParkingSpace/{SpaceNumber}/{ParkingId}")]
        public ActionResult GetParkingSpace(String SpaceNumber , int ParkingId)
        {
            if (ModelState.IsValid)
            {
                if (SpaceNumber != null)
                {
                    var Id = _spaces.GetIdParkingSpace(SpaceNumber, ParkingId);
                    if (Id == 0)
                    {
                        return BadRequest("parking space number incorrect");
                    }
                    else
                    {
                        var space = _spaces.GetParkingSpace(Id);
                        if (space == null)
                        {
                            return BadRequest("parking space number undefiend");
                        }
                        else
                        {
                            return Ok(space);
                        }
                    }
                }
                else
                {
                    return BadRequest("number is null");
                }
            }
            else
            {
                return BadRequest("system model invalid");
            }
        }
        //update parking space ststus to vacant by space number (which string)
        [HttpPut]
        [Route("VacantParkingSpace/{SpaceNumber}/{ParkingId}")]
        public ActionResult VacantParkingSpace(String parkingSpaceNumber, int ParkingId)
        {
            if (ModelState.IsValid)
            {
                var parkingId = _spaces.GetIdParkingSpace(parkingSpaceNumber, ParkingId);
                if (parkingId != 0)
                {
                    _spaces.VacantParkingSpace(parkingId);
                    return Ok("updated to vacant successfully");
                }
                else { return BadRequest("parking space number undefine"); }
            }
            else
            {
                return BadRequest("model is invalid");
            }
        }
        [HttpDelete]
        [Route("DeleteSpaceParking")]
        public ActionResult DeleteSpaceParking(int Id)
        {
            if (ModelState.IsValid)
            {
                if (Id > 0)
                {
                    _spaces.DeleteSpaceParking(Id);
                    return Ok("deleted successfully");
                }
                else { return BadRequest("id is invalid"); }
            }
            else { return BadRequest("model is invalid"); }
        }
        [HttpPost]
        [Route("UploudeCarPlateImage/{ParkingId}")]
        public ActionResult UploudeCarPlateImage(IFormFile image, int ParkingId)
        {
            String SpaceNumber = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
            if (_spaces.GetIdParkingSpace(SpaceNumber, ParkingId) == 0)
            {
                return BadRequest("space number undefiend");
            }
            else
            {
                _spaces.CarPlateImageStore(image);
                _spaces.OCR(image, ParkingId);
                _spaces.CarPlateImageRremove(image);

                return Ok("Space CarNumber Updated successfully");
            }


        }
        [HttpGet]
        [Route("CarFinder/{CarNumber}")]
        public ActionResult CarFinder(string CarNumber)
        {
            string SpaceNumber = _spaces.FindCarSpace(CarNumber);

            if(SpaceNumber == null)
            {
                return BadRequest("The car plate number is null");
            }
            else
            {
                return Ok(SpaceNumber);
            }


        }

    }

}