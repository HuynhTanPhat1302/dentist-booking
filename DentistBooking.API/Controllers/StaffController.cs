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
        public IActionResult GetStaffById(int id)
        {
           var staff = _staffService.GetStaffById(id);
           if (staff == null)
           {
               return NotFound();
           }
           return Ok(staff);
        }

        [HttpPost]
        public IActionResult CreateStaff(StaffApiModel staffApiModel)
        {
           var staff = _mapper.Map<staff>(staffApiModel);
           _staffService.CreateStaff(staff);
           return CreatedAtAction(nameof(GetStaffById), new { id = staff.StaffId }, staff);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateStaff(int id, staff staff)
        {
           if (id != staff.StaffId)
           {
               return BadRequest();
           }
           _staffService.UpdateStaff(staff);
           return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStaff(int id)
        {
           _staffService.DeleteStaff(id);
           return NoContent();
        }
    }
}
