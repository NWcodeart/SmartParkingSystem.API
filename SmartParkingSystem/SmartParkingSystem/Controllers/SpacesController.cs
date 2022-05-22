using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingSystem.ApplicationLayer.IRepositories;
using System;
using System.Drawing;
using Python.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartParkingSystem.Entity;
using System.IO;

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
        public ActionResult VacantParkingSpace(String SpaceNumber, int ParkingId)
        {
            if (ModelState.IsValid)
            {
                var parkingId = _spaces.GetIdParkingSpace(SpaceNumber, ParkingId);
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
        //update parking space ststus to vacant by space number (which string)
        [HttpPut]
        [Route("UnVacantParkingSpace/{SpaceNumber}/{ParkingId}")]
        public ActionResult UnVacantParkingSpace(String SpaceNumber, int ParkingId)
        {
            if (ModelState.IsValid)
            {
                var parkingId = _spaces.GetIdParkingSpace(SpaceNumber, ParkingId);
                if (parkingId != 0)
                {
                    _spaces.unVacantParkingSpace(parkingId);
                    return Ok("updated to unvacant successfully");
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
        public ActionResult UploudeCarPlateImage(int ParkingId, IFormFile image)
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
        [HttpPost]
        [Route("PostImageBase64")]
        public ActionResult PostImageBase64(ImageBase64 imageBase64)
        {
            if (imageBase64 == null) { return BadRequest("image is null"); }
            else {

                //decoding the image incoded in base64
                byte[] bytes = Convert.FromBase64String(imageBase64.image);
                MemoryStream stream = new MemoryStream(bytes);

                String fileName = imageBase64.SpaceNumber + ".jpeg";
                IFormFile image = new FormFile(stream, 0, bytes.Length, fileName, fileName);

                //image process
                _spaces.CarPlateImageStore(image);
                _spaces.OCR(image, imageBase64.ParkingId);
                _spaces.CarPlateImageRremove(image);
                return Ok("image posted successfully"); 
            }
        }

        [HttpGet]
        [Route("CarFinder/{CarNumber}/{parkingId}")]
        public ActionResult CarFinder(string CarNumber, int parkingId)
        {
            string SpaceNumber = _spaces.FindCarSpace(CarNumber , parkingId);

            if(SpaceNumber == null)
            {
                return BadRequest("The car plate number isn't defiend ");
            }
            else
            {
                return Ok(SpaceNumber);
            }


        }

    }

}