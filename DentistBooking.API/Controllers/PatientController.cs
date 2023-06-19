using System.Text;
using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpGet]
        [Route("get-patient-by-email/{email}")]
        public IActionResult GetPatientByEmail(string email)
        {
            if (email.Length <= 4 || email.Length >= 255)
            {
                return BadRequest();
            }
            var patient = _patientService.GetPatientByEmail(email);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        //search-patient (paging, sort alphabalet)
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<PatientApiRequestModel>>> SearchPatients(int pageSize, int pageNumber, string searchQuery)
        {
            // Validation parameter

            if (pageSize <= 0)
            {
                var response = new
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = 400,
                    Errors = new Dictionary<string, List<string>>
    {
        { "pageSize", new List<string> { "Page size must be greater than zero." } }
    }
                };

                return BadRequest(response);
            }

            if (pageNumber <= 0)
            {
                var response = new
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = 400,
                    Errors = new Dictionary<string, List<string>>
    {
        { "pageNumber", new List<string> { "Page number must be greater than zero." } }
    }
                };
                return BadRequest(response);
            }

            if (string.IsNullOrEmpty(searchQuery))
            {
                var response = new
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = 400,
                    Errors = new Dictionary<string, List<string>>
    {
        { "searchQuery", new List<string> { "seachQuery must not be null or empty." } }
    }
                };
                return BadRequest(response);
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
        public IActionResult CreatePatient(RegisterRequestModel registerRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = _mapper.Map<Patient>(registerRequestModel);



            // Make an HTTP request to the other project's API endpoint
            using (var httpClient = new HttpClient())
            {
                var createAccountUrl = "https://localhost:7214/api/auth/create-account";

                // Create an instance of the RegisterRequestModel based on the patient data
                var source1Model = new
                {
                    Email = registerRequestModel.Email,
                    Password = registerRequestModel.Password,
                    RoleID = registerRequestModel.RoleID
                    // Set other properties as needed
                };

                // Serialize the RegisterRequestModel to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(registerRequestModel), Encoding.UTF8, "application/json");

                // Send the POST request to the other project's API endpoint
                var response = httpClient.PostAsync(createAccountUrl, jsonContent).Result;

                if (!response.IsSuccessStatusCode)
                {
                    // Handle any errors from the other project's API
                    return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    try
                    {
                        _patientService.CreatePatient(patient);
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during patient creation
                        return StatusCode(500, ex);
                    }

                    return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, patient);
                }
            }


        }


        [HttpPut("{email}")]
        public IActionResult UpdatePatient(string email, [FromBody] PatientApiRequestModel patientApiRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = _patientService.GetPatientByEmail(email);
            if (patient == null)
            {
                return NotFound();
            }

            // Check if the email is being updated
            if (!string.Equals(patientApiRequestModel.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                // Make an HTTP request to update the account in the authentication source
                using (var httpClient = new HttpClient())
                {
                    var updateAccountUrl = $"https://localhost:7214/api/auth/update-account/{email}";

                    // Create an instance of the RegisterRequestModel based on the patient data
                    var updateAccountModel = new
                    {
                        Email = patientApiRequestModel.Email,
                        // Include other properties needed for account update
                    };

                    // Serialize the update model to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(updateAccountModel), Encoding.UTF8, "application/json");

                    // Send the PUT request to the authentication source's API endpoint
                    var response = httpClient.PutAsync(updateAccountUrl, jsonContent).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        // Handle any errors from the authentication source's API
                        return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
                    }
                }
            }

            // Use AutoMapper to map the properties from patientApiRequestModel to patient
            _mapper.Map(patientApiRequestModel, patient);

            try
            {
                _patientService.UpdatePatient(patient);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during patient update
                return StatusCode(500, ex);
            }

            return NoContent();
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeletePatient(string email)
        {
            var patient = await _patientService.GetPatientByEmailAsync(email);
            if (patient == null)
            {
                return NotFound();
            }

            try
            {
                if (patient.Email != null)
                {
                    await _patientService.DeletePatientAsync(patient.Email);
                }
                else
                {
                    return BadRequest("The Patient have no email");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during patient deletion
                return StatusCode(500, ex);
            }

            return NoContent();
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






        //[HttpDelete("DeletePatient/{id}")]
        //public IActionResult DeletePatient(int id)
        //{
        //    _patientService.DeletePatient(id);
        //    return NoContent();
        //}
    }
}





