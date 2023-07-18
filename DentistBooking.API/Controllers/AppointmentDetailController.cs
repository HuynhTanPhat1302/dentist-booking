using System.Text;
using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DentistBooking.API.Controllers
{
    [Route("api/appointment-detailes")]
    [ApiController]
    public class AppointmentDetailController : ControllerBase
    {
        private readonly IAppointmentDetailService _appointmentDetailService;
        private readonly IMapper _mapper;

        public AppointmentDetailController(IAppointmentDetailService appointmentDetailService, IMapper mapper)
        {
            _appointmentDetailService = appointmentDetailService;
            _mapper = mapper;
        }

        //get appointmentDetail by id
        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = "DentistOrStaff")]
        public IActionResult GetAppointmentDetailById(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var appointmentDetail = _appointmentDetailService.GetAppointmentDetailById(id);
            if (appointmentDetail == null)
            {
                return NotFound();
            }

            var appointmentDetailRespondModel = _mapper.Map<AppointmentDetailRespondModel>(appointmentDetail);
            return Ok(appointmentDetailRespondModel);
        }

        [Authorize(Policy = "DentistOrStaff")]
        [HttpGet("get-by-medical-record-id/{medicalRecordId}")]
        public async Task<IActionResult> GetAppointmentDetailsByMedicalRecordIdAsync(int medicalRecordId)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailService.GetAppointmentDetailsByMedicalRecordIdAsync(medicalRecordId);

                if (appointmentDetails == null)
                {
                    return NotFound();
                }

                var appointmentDetailRespondModels = _mapper.Map<IEnumerable<AppointmentDetailRespondModel>>(appointmentDetails);
                return Ok(appointmentDetailRespondModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("get-by-appointment-id/{appointmentId}")]
        [Authorize(Policy = "DentistOrStaff")]
        public async Task<IActionResult> GetAppointmentDetailsByAppointmentIdAsync(int appointmentId)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailService.GetAppointmentDetailByAppointmentIdAsync(appointmentId);

                if (appointmentDetails == null)
                {
                    return NotFound();
                }

                var appointmentDetailRespondModels = _mapper.Map<IEnumerable<AppointmentDetailRespondModel>>(appointmentDetails);
                return Ok(appointmentDetailRespondModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPost]
        [Authorize(Policy = "DentistOrStaff")]
        public IActionResult CreateAppointmentDetail([FromBody] AppointmentDetailCreateModel appointmentDetailCreateModel)
        {
            if (appointmentDetailCreateModel == null)
            {
                return BadRequest();
            }

            // Map the create model to an AppointmentDetail entity
            var newAppointmentDetail = _mapper.Map<AppointmentDetail>(appointmentDetailCreateModel);

            // Create the appointmentDetail
            _appointmentDetailService.CreateAppointmentDetail(newAppointmentDetail);

            // Return the newly created appointmentDetail as a response
            var createdAppointmentDetailRespondModel = _mapper.Map<AppointmentDetailRespondModel>(newAppointmentDetail);
            return CreatedAtAction(nameof(GetAppointmentDetailById), new { id = createdAppointmentDetailRespondModel.AppointmentDetailId }, createdAppointmentDetailRespondModel);
        }

        [HttpPost("add-medical-record-to-appointment")]
        [Authorize(Policy = "DentistOrStaff")]
        public IActionResult AddMedicalRecordToAppointment([FromBody] AppointmentDetailCreateModel appointmentDetailCreateModel)
        {
            if (appointmentDetailCreateModel == null)
            {
                return BadRequest();
            }

            var newAppointmentDetail = _mapper.Map<AppointmentDetail>(appointmentDetailCreateModel);


            try
            {
                _appointmentDetailService.AddMedicalRecordToAppointment(newAppointmentDetail);
                var createdAppointmentDetail = _appointmentDetailService.GetAppointmentDetailById(newAppointmentDetail.AppointmentDetailId);
                return CreatedAtAction(nameof(GetAppointmentDetailById), new { id = newAppointmentDetail.AppointmentDetailId }, createdAppointmentDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = "DentistOrStaff")]
        public IActionResult UpdateAppointmentDetail(int id, [FromBody] AppointmentDetailCreateModel appointmentDetailCreateModel)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }

            if (appointmentDetailCreateModel == null)
            {
                return BadRequest();
            }

            var existingAppointmentDetail = _appointmentDetailService.GetAppointmentDetailById(id);
            if (existingAppointmentDetail == null)
            {
                return NotFound();
            }

            // Update the existing appointmentDetail with the new data

            _mapper.Map(appointmentDetailCreateModel, existingAppointmentDetail);


            // Save the updated appointmentDetail
            _appointmentDetailService.UpdateAppointmentDetail(existingAppointmentDetail);

            // Return the updated appointmentDetail as a response
            var updatedAppointmentDetailRespondModel = _mapper.Map<AppointmentDetailRespondModel>(existingAppointmentDetail);
            return Ok(updatedAppointmentDetailRespondModel);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = "DentistOrStaff")]
        public IActionResult DeleteAppointmentDetail(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var existingAppointmentDetail = _appointmentDetailService.GetAppointmentDetailById(id);
            if (existingAppointmentDetail == null)
            {
                return NotFound();
            }
            // Delete the appointmentDetail
            _appointmentDetailService.DeleteAppointmentDetail(id);
            return NoContent();
        }


    }
}





