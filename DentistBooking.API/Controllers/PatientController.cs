using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Data;

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IProposeAppointmentService _proposeAppointmentService;
        private readonly ITreatmentService _treatmentService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public PatientController(IPatientService patientService, IMapper mapper, 
            IProposeAppointmentService proposeAppointmentService, ITreatmentService treatmentService
            , IMedicalRecordService medicalRecordService, IAppointmentService appointmentService)
        {
            _patientService = patientService;
            _mapper = mapper;
            _proposeAppointmentService = proposeAppointmentService;
            _treatmentService = treatmentService;
            _medicalRecordService = medicalRecordService;
            _appointmentService = appointmentService;
        }

        //get all patients
        //[HttpGet("GetPatients")]

        //public IActionResult GetAllPatients()
        //{
        //    var patients = _patientService.GetAllPatients();
        //    return Ok(patients);
        //}

        //get all treatments and its price
        //[HttpGet("GetTreatments")]
        //public IActionResult GetAllTreatments()
        //{
        //    var treatments = _treatmentService.GetAllTreatments();
        //    var treatmentApiRequestModel = _mapper.Map<List<TreatmentApiRequestModel>>(treatments);
        //    return Ok(treatmentApiRequestModel);
        //}

        //get patient by id
        //[HttpGet("GetPatientByID/{id}")]
        //public IActionResult GetPatientById(int id)
        //{
        //    var patient = _patientService.GetPatientById(id);
        //    if (patient == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(patient);
        //}

        //create new patient
        //[HttpPost("CreatePatient")]
        //public IActionResult CreatePatient(PatientApiModel patientApiModel)
        //{
        //    var patient = _mapper.Map<Patient>(patientApiModel);
        //    _patientService.CreatePatient(patient);
        //    return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, patient);
        //}

        [HttpGet("GetProposeAppointmentById/{id}")]
        public IActionResult GetProposeAppointmentById(int id)
        {
            var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);
            if (proposeAppointment == null)
            {
                return NotFound();
            }
            return Ok(proposeAppointment);
        }

        [HttpPost("CreateProposeAppointment")]
        public IActionResult CreateProposeAppointment(ProposeAppointmentRequestModel proposeAppointmentRequestModel)
        {
            var proposeAppointment = _mapper.Map<ProposeAppointment>(proposeAppointmentRequestModel);
            _proposeAppointmentService.CreateProposeAppointment(proposeAppointment);
            
            return CreatedAtAction(nameof(GetProposeAppointmentById), new { id = proposeAppointment.ProposeAppointmentId }, proposeAppointment);
        }

        [HttpGet("GetMedicalRecordByPatientEmail/{email}")]
        public IActionResult GetMedicalRecordByEmail(string email)
        {
                var medicalRecords = _medicalRecordService.GetMedicalRecordsByPatientEmail(email);

                if (medicalRecords == null)
                {
                    
                    return NotFound();
                }

                var medicalRecordApiRequestModels = _mapper.Map<List<MedicalRecordApiRequestModel>>(medicalRecords);

                
                return Ok(medicalRecordApiRequestModels);
        }

        [HttpGet("GetAppointmentsByPatientEmail/{email}")]
        public IActionResult GetAppointmentsByPatientEmail(string email)
        {
            var appointments = _appointmentService.GetAppointmentsByPatientEmail(email);

            if (appointments == null)
            {
                return NotFound();
            }

            var appointmentApiRequestModel = _mapper.Map<List<AppointmentApiRequestModel>>(appointments);


            return Ok(appointmentApiRequestModel);
        }




        //[HttpPut("UpdatePatient/{id}")]
        //public IActionResult UpdatePatient(int id, Patient patient)
        //{
        //    if (id != patient.PatientId)
        //    {
        //        return BadRequest();
        //    }
        //    _patientService.UpdatePatient(patient);
        //    return NoContent();
        //}

        //[HttpDelete("DeletePatient/{id}")]
        //public IActionResult DeletePatient(int id)
        //{
        //    _patientService.DeletePatient(id);
        //    return NoContent();
        //}
    }
}





