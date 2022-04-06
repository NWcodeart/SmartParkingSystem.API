using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingSystem.Entity;
using SmartParkingSystem.ApplicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParkingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private IParking _iparkingRepo;
        public ParkingController(IParking parkingReop)
        {
            _iparkingRepo = parkingReop;
        }
        [HttpPost]
        [Route("CreateCompanyParking")]
        public IActionResult CreateCompanyParking(AddParkingDto parking)
        {
            if (ModelState.IsValid)
            {
                if (parking == null)
                {
                    return BadRequest("Building NULL");
                }
                else
                {
                    _iparkingRepo.AddCompanyParking(parking);
                    return Ok("company parking added successfully");
                }

            }
            else
            {
                return BadRequest("Model State is invalid");
            }
        }

        [HttpGet]
        [Route("GetCompanyParking")]
        public IActionResult GetAllCompanyParking()
        {
            if (ModelState.IsValid)
            {
                var companyParkingList = _iparkingRepo.GetAllCompanyParking();
                if (companyParkingList != null)
                {
                    return Ok(companyParkingList);
                }
                else
                {
                    return BadRequest("no company parking yet");
                }
            }
            else
            {
                return BadRequest("Model State is invalid");
            }
        }

        [HttpGet]//("{id:int}")
        [Route("GetCompanyParkingById/{Id}")]
        public IActionResult GetAllCompanyParkingById(int Id)
        {
            if (ModelState.IsValid)
            {
                var companyParking = _iparkingRepo.GetCompanyParking(Id);
                if (companyParking != null)
                {
                    return Ok(companyParking);
                }
                else
                {
                    return BadRequest("there is no company parking have this id");
                }
            }
            else
            {
                return BadRequest("Model State is invalid");
            }
        }

        [HttpDelete]
        [Route("DeleteCompanyParking")]
        public IActionResult DeleteCompanyParking(int parkingId)
        {
            if (ModelState.IsValid)
            {
                if (parkingId == 0)
                {
                    return BadRequest("Building NULL");
                }
                else
                {
                    _iparkingRepo.DeleteCompanyParking(parkingId);
                    return Ok("company parking Deleted successfully");
                }

            }
            else
            {
                return BadRequest("Model State is invalid");
            }
        }

        [HttpPost]
        [Route("AddPrkingSpace")]
        public IActionResult AddPrkingSpace(AddSpaceDto space)
        {
            if (ModelState.IsValid)
            {
                if (space != null)
                {
                    _iparkingRepo.AddParkingSpace(space);
                    return Ok("added");
                }
                else
                {
                    return BadRequest("space is null");
                }

            }
            else
            {
                return BadRequest("Model State is invalid");
            }
        }
    }
}
