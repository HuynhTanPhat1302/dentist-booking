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

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "StaffOnly")]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly IMapper _mapper;

        public StaffsController(IStaffService staffService, IMapper mapper)
        {
            _staffService = staffService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllStaffs()
        {
            var staffs = _staffService.GetAllStaffs();
            return Ok(staffs);
        }

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

        [HttpGet("view-all-patients")]
        public IActionResult ViewAllPatients()
        {
            try
            {
                var patients = _staffService.GetAllPatients();
                var patientsDto = _mapper.Map<List<PatientApiModel>>(patients);
                if (patients == null)
                {
                    throw new Exception("The list is empty");
                }
                return Ok(patientsDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


        }

        [HttpGet("view-patient-details/{id}")]
        public IActionResult ViewPatientDetails(int id)
        {
            try
            {
                var patient = _staffService.GetPatient(id);
                var patientDTO = _mapper.Map<PatientApiModel>(patient);
                if (patient == null)
                {
                    throw new Exception("Patient is not existed!");
                }
                return Ok(patientDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("get-medical-records/{id}")]
        public IActionResult GetMedicalRecords(int id)
        {
            try
            {
                var medicalRecords = _staffService.GetMedicalRecords(id);
                var medicalRecordsDTO = _mapper.Map<List<MedicalRecordsApiModel>>(medicalRecords);
                if (medicalRecords == null)
                {
                    throw new Exception("Patient is not existed!");
                }
                return Ok(medicalRecordsDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("create-an-appointment")]
        public IActionResult CreateAnAppointment([FromBody] AppointmentApiModelRequest appointment)
        {
            try
            {
                var appointmentResponse = _staffService.CreateAnAppointment(_mapper.Map<Appointment>(appointment));
                if (appointmentResponse == null)
                {
                    throw new Exception("Booking time is existed");
                }
                var res = _mapper.Map<AppointmentApiModel>(appointmentResponse);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("create-an-patient-account")]
        public IActionResult CreateAccountOfPatient([FromBody] PatientApiModelRequest patientRequest)
        {
            try
            {
                var res = _staffService.CreateAccountOfPatient(_mapper.Map<Patient>(patientRequest));
                return Ok(_mapper.Map<PatientApiModel>(res));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
