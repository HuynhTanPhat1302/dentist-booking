using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly IMapper _mapper;

        public StaffController(IStaffService staffService, IMapper mapper)
        {
            _staffService = staffService;
            _mapper = mapper;
        }

        // GET: api/<StaffController>
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

        // GET api/<StaffController>/5
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

        // POST api/<StaffController>
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        
    }
}
