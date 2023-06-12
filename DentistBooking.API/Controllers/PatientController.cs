using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace DentistBooking.API.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public PatientController(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        //get patient by id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPatientById(int id)
        {
            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        //[HttpGet]
        //[Route("{email}")]
        //public IActionResult GetPatientByEmail(string email)
        //{
        //    var patient = _patientService.GetPatientByEmail(email);
        //    if (patient == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(patient);
        //}

        //search-patient (paging, sort alphabalet)
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<PatientApiRequestModel>>> SearchPatients(int pageSize, int pageNumber, string searchQuery)
        {
            // Validation parameter

            if (pageSize <= 0)
            {
                return BadRequest("Page size must be greater than zero.");
            }

            if (pageNumber <= 0)
            {
                return BadRequest("Page number must be greater than zero.");
            }

            if (string.IsNullOrEmpty(searchQuery))
            {
                return BadRequest("seachQuery must not be null or empty.");
            }

            List<Patient> patients;
            if (string.IsNullOrEmpty(searchQuery))
            {
                patients = await _patientService.GetPatientsAsync(pageSize, pageNumber);
            }
            else
            {
                patients = await _patientService.SearchPatientsAsync(pageSize, pageNumber, searchQuery);
            }

            var patientApiRequestModels = _mapper.Map<List<PatientApiRequestModel>>(patients);
            return patientApiRequestModels;
        }

        [HttpPost]
        public IActionResult CreatePatient(PatientApiRequestModel patientApiRequestModel)
        {
            var patient = _mapper.Map<Patient>(patientApiRequestModel);
            _patientService.CreatePatient(patient);
            return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, patient);
        }


        //[HttpPut("{id}")]
        //public IActionResult UpdatePatient(int id, staff staff)
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



        //create new patient
        //[HttpPost("CreatePatient")]
        //public IActionResult CreatePatient(PatientApiModel patientApiModel)
        //{
        //    var patient = _mapper.Map<Patient>(patientApiModel);
        //    _patientService.CreatePatient(patient);
        //    return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, patient);
        //}

        //[HttpGet("GetProposeAppointmentById/{id}")]
        //public IActionResult GetProposeAppointmentById(int id)
        //{
        //    var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);
        //    if (proposeAppointment == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(proposeAppointment);
        //}

        //[HttpPost("CreateProposeAppointment")]
        //public IActionResult CreateProposeAppointment(ProposeAppointmentRequestModel proposeAppointmentRequestModel)
        //{
        //    var proposeAppointment = _mapper.Map<ProposeAppointment>(proposeAppointmentRequestModel);
        //    _proposeAppointmentService.CreateProposeAppointment(proposeAppointment);

        //    return CreatedAtAction(nameof(GetProposeAppointmentById), new { id = proposeAppointment.ProposeAppointmentId }, proposeAppointment);
        //}

        //[HttpGet("GetMedicalRecordByPatientEmail/{email}")]
        //public IActionResult GetMedicalRecordByEmail(string email)
        //{
        //        var medicalRecords = _medicalRecordService.GetMedicalRecordsByPatientEmail(email);

        //        if (medicalRecords == null)
        //        {

        //            return NotFound();
        //        }

        //        var medicalRecordApiRequestModels = _mapper.Map<List<MedicalRecordApiRequestModel>>(medicalRecords);


        //        return Ok(medicalRecordApiRequestModels);
        //}

        //[HttpGet("GetAppointmentsByPatientEmail/{email}")]
        //public IActionResult GetAppointmentsByPatientEmail(string email)
        //{
        //    var appointments = _appointmentService.GetAppointmentsByPatientEmail(email);

        //    if (appointments == null)
        //    {
        //        return NotFound();
        //    }

        //    var appointmentApiRequestModel = _mapper.Map<List<AppointmentApiRequestModel>>(appointments);


        //    return Ok(appointmentApiRequestModel);
        //}




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





