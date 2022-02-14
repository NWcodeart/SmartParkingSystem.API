using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingSystem.ApplicationLayer.IRepositories;
using System;
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
        [Route("GetParkingSpace")]
        public ActionResult GetParkingSpace(String SpaceNumber)
        {
            if (ModelState.IsValid)
            {
                if (SpaceNumber != null)
                {
                    var Id = _spaces.GetIdParkingSpace(SpaceNumber);
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

        [HttpPut]
        [Route("VacantParkingSpace")]
        public ActionResult VacantParkingSpace(String parkingSpaceNumber)
        {
            if (ModelState.IsValid)
            {
                var parkingId = _spaces.GetIdParkingSpace(parkingSpaceNumber);
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
        [Route("UploudeCarPlateImage")]
        public ActionResult UploudeCarPlateImage(IFormFile image)
        {
            String SpaceNumber = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
            if (_spaces.GetIdParkingSpace(SpaceNumber) == 0)
            {
                return BadRequest("space number undefiend");
            }
            else
            {
                _spaces.CarPlateImageStore(image);
                _spaces.OCR(image);
                _spaces.CarPlateImageRremove(image);

                return Ok("Space CarNumber Updated successfully");
            }


        }

    }

}