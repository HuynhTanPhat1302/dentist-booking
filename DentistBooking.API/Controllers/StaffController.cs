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
    /*[Authorize(Policy = "StaffOnly")]*/
    [AllowAnonymous]
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
        //public IActionResult GetAllStaffs()
        //{
        //    var staffs = _staffService.GetAllStaffs();
        //    return Ok(staffs);
        //}

        //[HttpGet("{id}")]
        //public IActionResult GetStaffById(int id)
        //{
        //    var staff = _staffService.GetStaffById(id);
        //    if (staff == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(staff);
        //}

        //[HttpPost]
        //public IActionResult CreateStaff(StaffApiModel staffApiModel)
        //{
        //    var staff = _mapper.Map<staff>(staffApiModel);
        //    _staffService.CreateStaff(staff);
        //    return CreatedAtAction(nameof(GetStaffById), new { id = staff.StaffId }, staff);
        //}


        //[HttpPut("{id}")]
        //public IActionResult UpdateStaff(int id, staff staff)
        //{
        //    if (id != staff.StaffId)
        //    {
        //        return BadRequest();
        //    }
        //    _staffService.UpdateStaff(staff);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeleteStaff(int id)
        //{
        //    _staffService.DeleteStaff(id);
        //    return NoContent();
        //}

        [HttpGet("view-all-patients")]
        public IActionResult ViewAllPatients()
        {
            try
            {
                var patients = _staffService.GetAllPatients();
                var patientsDto = _mapper.Map<List<PatientApiModel>>(patients);
                if (patients.Count == 0)
                {
                    throw new Exception("The list is empty");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Patients retrieved successfully",
                    Data = patientsDto
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "The list is empty",
                    Error = ex.Message
                };
                return NotFound(response);
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
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Patients retrieved successfully",
                    Data = patientDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Patient is not existed!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }

        [HttpGet("view-medical-records/{id}")]
        public IActionResult GetMedicalRecords(int id)
        {
            try
            {
                var medicalRecords = _staffService.GetMedicalRecords(id);
                var medicalRecordsDTO = _mapper.Map<MedicalRecordApiRequestModel>(medicalRecords);
                if (medicalRecords == null)
                {
                    throw new Exception("Patient is not existed!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "MedicalRecords retrieved successfully",
                    Data = medicalRecordsDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Medical Records is not existed!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }

        [HttpGet("view-medical-records-of-patient/{patientId}")]
        public IActionResult GetMedicalRecordsOfPatient(int patientId)
        {
            try
            {
                var medicalRecords = _staffService.GetMedicalRecordsOfPatient(patientId);
                var medicalRecordsDTO = _mapper.Map<List<MedicalRecordApiRequestModel>>(medicalRecords);
                if (medicalRecords.Count == 0)
                {
                    throw new Exception("Patient is not existed!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "MedicalRecords retrieved successfully",
                    Data = medicalRecordsDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Patient is not existed!!!",
                    Error = ex.Message
                };
                return NotFound(response);
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
                return CreatedAtAction(nameof(GetAnAppointment), new { id = res.AppointmentId }, res);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Create unsuccesfully",
                    Error = ex.Message
                };
                return BadRequest(response);
            }

        }

        [HttpGet("view-an-appointment/{id}")]
        public IActionResult GetAnAppointment(int id)
        {
            try
            {
                var appointment = _staffService.GetAppointment(id);
                if (appointment == null)
                {
                    throw new Exception("Appointment is not existed!!!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Appointment retrieved successfully",
                    Data = _mapper.Map<AppointmentApiModel>(appointment)
                };
                return Ok(response);
            } catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Appointment is not existed!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }

        [HttpPost("create-an-patient-account")]
        public IActionResult CreateAccountOfPatient([FromBody] PatientApiModelRequest patientRequest)
        {
            try
            {
                var res = _staffService.CreateAccountOfPatient(_mapper.Map<Patient>(patientRequest));
                var patient = _mapper.Map<PatientApiModel>(res);
                return CreatedAtAction(nameof(ViewPatientDetails), new { id = patient.PatientId}, patient);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Create unsuccesfully",
                    Error = ex.Message
                };
                return BadRequest(response);
            }
        }

        [HttpGet("view-all-appointments")]
        public IActionResult GetAllBookingAppointment()
        {
            try
            {
                var res = _staffService.GetAllAppointments();
                if (res.Count == 0)
                {
                    throw new Exception("The list is empty!!!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Appointments retrieved successfully",
                    Data = _mapper.Map<List<AppointmentApiModel>>(res)
                };
                return Ok(response);
            }catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "The list is empty!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }
    }
}
