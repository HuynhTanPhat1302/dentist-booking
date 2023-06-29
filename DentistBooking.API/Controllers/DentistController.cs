using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistController : ControllerBase
    {
        private readonly IDentistService _dentistService;

        private readonly IMapper _mapper;

        public DentistController(IMapper mapper,
            IDentistService dentistService)
        {
            _mapper = mapper;
            _dentistService = dentistService;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDentistAccountById(int id)
        {
            try
            {
                var dentist = _dentistService.GetDentistById(id);
                if (dentist == null)
                {
                    throw new Exception("Dentist is not existed!!!");
                }
                return Ok(_mapper.Map<DentistApiModel>(dentist));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Dentist is not existed",
                    Error = ex.Message
                };
                return NotFound(response);
            }


        }


        //search-patient (paging, sort alphabalet)
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<DentistApiModel>>> SearchPatients(int pageSize, int pageNumber, string searchQuery)
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

            List<Dentist> dentists;
            if (string.IsNullOrEmpty(searchQuery))
            {
                dentists = await _dentistService.GetDentistsAsync(pageSize, pageNumber);
            }
            else
            {
                dentists = await _dentistService.SearchDentistsAsync(pageSize, pageNumber, searchQuery);
            }

            var patientApiRequestModels = _mapper.Map<List<DentistApiModel>>(dentists);
            return patientApiRequestModels;
        }

        [HttpPost]
        public IActionResult CreateDentistAccount([FromBody] RegisterRequestModel dentistAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Make an HTTP request to the other project's API endpoint
            using (var httpClient = new HttpClient())
            {
                var createAccountUrl = "https://localhost:7214/api/auth/create-account";

                // Create an instance of the RegisterRequestModel based on the patient data
                var source1Model = new
                {
                    Email = dentistAccount.Email,
                    Password = dentistAccount.Password,
                    RoleID = dentistAccount.RoleID
                    // Set other properties as needed
                };

                // Serialize the RegisterRequestModel to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(dentistAccount), Encoding.UTF8, "application/json");

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
                        var dentist = _dentistService.CreateDentist(_mapper.Map<Dentist>(dentistAccount));
                        var res = _mapper.Map<DentistApiModel>(dentist);
                        return CreatedAtAction(nameof(GetDentistAccountById), new { id = res.DentistId }, res);
                    }
                    catch (Exception ex)
                    {
                        var responseRes = new
                        {
                            ContentType = "application/json",
                            Success = false,
                            Message = "Create unsuccesfully",
                            Error = ex.Message
                        };

                        return BadRequest(responseRes);
                    }
                }
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDentistAccount([FromBody] DentistAccountApiModel dentistAccount, int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dentistIsExisted = _dentistService.GetDentistById(id);
            if (dentistIsExisted == null)
            {
                return NotFound();
            }

            // Check if the email is being updated
            if (!string.Equals(dentistAccount.Email, dentistIsExisted.Email, StringComparison.OrdinalIgnoreCase))
            {
                // Make an HTTP request to update the account in the authentication source
                using (var httpClient = new HttpClient())
                {
                    var updateAccountUrl = $"https://localhost:7214/api/auth/update-account/{dentistIsExisted.Email}";

                    // Create an instance of the RegisterRequestModel based on the patient data
                    var updateAccountModel = new
                    {
                        Email = dentistAccount.Email,
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

            try
            {
                var dentist = _dentistService.UpdateDentist(id, _mapper.Map<Dentist>(dentistAccount));
                var res = _mapper.Map<DentistApiModel>(dentist);
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Update successfully",
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
                    Message = "Create unsuccesfully",
                    Error = ex.Message
                };

                return BadRequest(response);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDentistAccount(int id)
        {
            try
            {
                _dentistService.DeleteDentist(id);
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Delete successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Delete unsuccesfully",
                    Error = ex.Message
                };

                return BadRequest(response);
            }
        }
    }
}
