using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "DentistOrStaff")]
        public IActionResult GetDentistAccountById(int id)
        {
            try
            {
                var dentist = _dentistService.GetDentistById(id);
                if (dentist == null)
                {
                    throw new Exception("Dentist is not existed!!!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Dentist retrieved successfully",
                    Data = dentist
                };
                return Ok(response);
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

        [HttpPost]
        [Authorize(Policy = "DentistOrStaff")]
        public IActionResult CreateDentistAccount([FromBody] DentistAccountApiModel dentistAccount)
        {
            using (var httpClient = new HttpClient())
            {
                var createAccountUrl = "http://user-management:80/api/auth/create-account";

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
                        // Handle any exceptions that occur during patient creation
                        return StatusCode(500, ex);
                    }
                }
            }
        }

        [HttpPut("{email}")]
        [Authorize(Policy = "DentistOrStaff")]
        public IActionResult UpdateDentistAccount(string email, [FromBody] DentistAccountApiModel dentistAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = _dentistService.GetDentistByEmail(email);
            if (patient == null)
            {
                return NotFound();
            }

            // Check if the email is being updated
            if (!string.Equals(dentistAccount.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                // Make an HTTP request to update the account in the authentication source
                using (var httpClient = new HttpClient())
                {
                    var updateAccountUrl = $"http://user-management:80/api/auth/update-account/{email}";

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

            // Use AutoMapper to map the properties from patientApiRequestModel to patient
            _mapper.Map(dentistAccount, patient);

            try
            {
                _dentistService.UpdateDentist(patient.DentistId, patient);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during patient update
                return StatusCode(500, ex);
            }

            return NoContent();
            /*try
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
            }*/
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DentistOrStaff")]
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
