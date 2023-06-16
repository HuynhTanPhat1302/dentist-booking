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
        public IActionResult ViewPatientDetails(int id)
        {
            try
            {
                var patient = _patientService.GetPatientById(id);
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
        public IActionResult CreateAccountOfPatient([FromBody] PatientApiRequestModel patientRequest)
        {
            try
            {
                var res = _patientService.CreateAccountOfPatient(_mapper.Map<Patient>(patientRequest));
                var patient = _mapper.Map<PatientApiModel>(res);
                return CreatedAtAction(nameof(ViewPatientDetails), new { id = patient.PatientId }, patient);
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
        //ViewAllPatient
        [HttpGet]
        public IActionResult ViewAllPatients()
        {
            try
            {
                var patients = _patientService.GetAllPatients().ToList();
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

        [HttpPut("{id}")]
        public IActionResult UpdatePatientInfor(int id, PatientApiRequestModel patient)
        {
            try
            {
                var patientRes = _patientService.UpdatePatient(id, _mapper.Map<Patient>(patient));
                var res = _mapper.Map<PatientApiModel>(patientRes);
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Patients updated",
                    Data = res
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Patients is not updated",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }
    }
}





