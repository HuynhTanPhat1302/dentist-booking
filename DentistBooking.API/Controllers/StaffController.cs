using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace DentistBooking.API.Controllers
{
    [Route("api/staffs")]
    [ApiController]
    /*[Authorize(Policy = "StaffOnly")]*/
    
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly IMapper _mapper;

        public StaffController(IStaffService staffService, IMapper mapper)
        {
            _staffService = staffService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("get-own-staff")]
        [Authorize(Policy = "StaffOnly")]
        public IActionResult GetStaffByEmail()
        {
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid token, email not found.");
            }
            
            var staff = _staffService.GetStaffByEmail(email);
            if (staff == null)
            {
                return NotFound();
            }
            return Ok(staff);
        }

        //[HttpGet]
        //public IActionResult GetAllStaffs()
        //{
        //    var staffs = _staffService.GetAllStaffs();
        //    return Ok(staffs);
        //}

        [HttpGet("{id}")]
        [Authorize(Policy = "StaffOnly")]
        public IActionResult GetStaffById(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var existingStaff = _staffService.GetStaffById(id);
            if (existingStaff == null)
            {
                return NotFound();
            }
            var staffRespondModel = _mapper.Map<StaffRespondModel>(existingStaff);
            return Ok(staffRespondModel);
        }

        [HttpPost]
        [Authorize(Policy = "StaffOnly")]
        public IActionResult CreateStaff(StaffApiModel staffApiModel)
        {
           var staff = _mapper.Map<staff>(staffApiModel);
           _staffService.CreateStaff(staff);
           return CreatedAtAction(nameof(GetStaffById), new { id = staff.StaffId }, staff);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "StaffOnly")]
        public IActionResult UpdateStaff(int id, [FromBody] StaffRespondModel staffRespondModel)
        {
            if (id != staffRespondModel.StaffId)
            {
                return BadRequest();
            }

            var existingStaff = _staffService.GetStaffById(id);
            if (existingStaff == null)
            {
                return NotFound();
            }

            // Update the existing staff with the new data
            _mapper.Map(staffRespondModel, existingStaff);

            // Save the updated staff
            _staffService.UpdateStaff(existingStaff);

            var updatedStaffRespondModel = _mapper.Map<StaffRespondModel>(existingStaff);
            return Ok(updatedStaffRespondModel);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "StaffOnly")]
        public IActionResult DeleteStaff(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var existingStaff = _staffService.GetStaffById(id);
            if (existingStaff == null)
            {
                return NotFound();
            }
            //Delete staff
            _staffService.DeleteStaff(id);
           return NoContent();
        }
    }
}
