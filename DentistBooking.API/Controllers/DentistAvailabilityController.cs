using System.Net;
using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistAvailability;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "DentistOrStaff")]
    public class DentistAvailabilityController : ControllerBase
    {
        private readonly IDentistAvailabilityService _dentistAvailabilityService;
        private readonly IMapper _mapper;

        public DentistAvailabilityController(IDentistAvailabilityService dentistAvailabilityService, IMapper mapper)
        {
            _dentistAvailabilityService = dentistAvailabilityService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<DentistAvailabilityModel>>> SearchDentistAvailability(int pageSize, int pageNumber, string? searchQuery)
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

            /*if (string.IsNullOrEmpty(searchQuery))
            {
                return BadRequest("seachQuery must not be null or empty.");
            }*/

            List<DentistAvailability> dentistAvailabilities;
            if (string.IsNullOrEmpty(searchQuery))
            {
                dentistAvailabilities = await _dentistAvailabilityService.GetDentistAvailabilitys(pageSize, pageNumber);
            }
            else
            {
                dentistAvailabilities = await _dentistAvailabilityService.SearchDentistAvailabilitysAsync(pageSize, pageNumber, searchQuery);
            }

            var patientApiRequestModels = _mapper.Map<List<DentistAvailabilityModel>>(dentistAvailabilities);
            return patientApiRequestModels;
        }

        // GET api/<DentistAvailabilityController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var dentistAvailability = _dentistAvailabilityService.GetById(id);
                var dentistAvailabilityDTO = _mapper.Map<DentistAvailabilityModel>(dentistAvailability);
                if (dentistAvailability == null)
                {
                    throw new Exception("Patient is not existed!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "DentistAvailability retrieved successfully",
                    Data = dentistAvailabilityDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "DentistAvailability is not existed!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }

        // POST api/<DentistAvailabilityController>
        [HttpPost]
        public IActionResult Post([FromBody] DentistAvailabilityRequestModel dentistAvailabilityRequestModel)
        {
            try
            {
                var dentistAvailability = _dentistAvailabilityService.CreateDentistAvailabilitys(_mapper.Map<DentistAvailability>(dentistAvailabilityRequestModel));
                var res = _mapper.Map<DentistAvailabilityModel>(dentistAvailability);
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "DentistAvailability updated",
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
                    Message = "DentistAvailability is not updated",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }

        // PUT api/<DentistAvailabilityController>/5
        // day of week not changed
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DentistAvailabilityRequestModel dentistAvailabilityRequestModel)
        {
            try
            {
                var dentistAvailability = _dentistAvailabilityService.UpdateDentistAvailabilitys(id, _mapper.Map<DentistAvailability>(dentistAvailabilityRequestModel));
                var res = _mapper.Map<DentistAvailabilityModel>(dentistAvailability);
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "DentistAvailability updated",
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
                    Message = "DentistAvailability is not updated",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }

        // DELETE api/<DentistAvailabilityController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _dentistAvailabilityService.DeleteDentistAvailability(id);
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "DentistAvailability deleted"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "DentistAvailability can not be deleted",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }

        // GET api/<DentistAvailabilityController>/5
        [HttpGet("dentist-freeTime/{date}")]
        [Authorize(Policy = "StaffOnly")]
        public async Task<IActionResult> GetDentistTimeAvailability(DateTime date)
        {
            try
            {
                var dentistAvailability = await _dentistAvailabilityService.GetDentistFreeTimeAvailability(date);
                if (dentistAvailability == null)
                {
                    throw new Exception("Dentist is not existed!");
                }
                var convert = ConvertDictionary(dentistAvailability);
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    TypeNameHandling = TypeNameHandling.None, // Don't include type names in JSON
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    Formatting = Formatting.None // Remove indentation for compact format
                };
                var responseJson = JsonConvert.SerializeObject(convert, settings);

                // Return the response with status code 200
                //return Content(responseJson, "application/json");
                var contentResult = new ContentResult
        {
            Content = responseJson,
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.OK
        };

        return contentResult;

            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "DentistAvailability is not existed!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }
        private Dictionary<string, List<TimeSlot>> ConvertDictionary(Dictionary<string, List<(TimeSpan Start, TimeSpan End)>> originalDictionary)
        {
            var convertedDictionary = new Dictionary<string, List<TimeSlot>>();

            foreach (var kvp in originalDictionary)
            {
                var dentistName = kvp.Key;
                var timeSlots = kvp.Value;

                var timeSlotList = timeSlots.Select(ts => new TimeSlot(ts.Start, ts.End)).ToList();
                convertedDictionary.Add(dentistName, timeSlotList);
            }

            return convertedDictionary;
        }
    }
}
