using System.Text;
using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DentistBooking.API.Controllers
{
    [Route("api/illnesses")]
    [ApiController]
    [Authorize(Policy = "DentistOnly")]
    public class IllnessController : ControllerBase
    {
        private readonly IIllnessService _illnessService;
        private readonly IMapper _mapper;

        public IllnessController(IIllnessService illnessService, IMapper mapper)
        {
            _illnessService = illnessService;
            _mapper = mapper;
        }

        //get illness by id
        [HttpGet]
        [Route("{id}")]        
        public IActionResult GetIllnessById(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var illness = _illnessService.GetIllnessById(id);
            if (illness == null)
            {
                return NotFound();
            }
            var illnessRespondModel = _mapper.Map<IllnessRespondModel>(illness);
            return Ok(illnessRespondModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetIllnesses(int pageSize = 10, int pageNumber = 1)
        {
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

            var illnesses = await _illnessService.GetIllnessesAsync(pageSize, pageNumber);
            if (illnesses == null)
            {
                return NotFound();
            }
            var illnessesRespondModel = _mapper.Map<List<IllnessRespondModel>>(illnesses);


            return Ok(illnessesRespondModel);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<IllnessRespondModel>>> SearchPatients(string searchQuery, int pageSize = 10, int pageNumber = 1)
        {
            // Validation parameter

            if (pageSize <= 0 || pageSize > 200)
            {
                var response = new
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = 400,
                    Errors = new Dictionary<string, List<string>>
    {
        { "pageSize", new List<string> { "Page size must be greater than zero and smaller than 201." } }
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

            List<Illness> illnesses;
            if (string.IsNullOrEmpty(searchQuery))
            {
                illnesses = await _illnessService.GetIllnessesAsync(pageSize, pageNumber);
            }
            else
            {
                illnesses = await _illnessService.SearchIllnesssAsync(pageSize, pageNumber, searchQuery);
            }

            var illnessesRespondModels = _mapper.Map<List<IllnessRespondModel>>(illnesses);
            return illnessesRespondModels;
        }

        // HTTP POST - Create a new illness
        [HttpPost]
        public IActionResult CreateIllness([FromBody] IllnessRequestModel illnessRequestModel)
        {
            if (illnessRequestModel == null)
            {
                return BadRequest();
            }

            // Map the create model to an Illness entity
            var newIllness = _mapper.Map<Illness>(illnessRequestModel);

            // Create the illness
            _illnessService.CreateIllness(newIllness);

            // Return the newly created illness as a response
            var createdIllnessRespondModel = _mapper.Map<IllnessRespondModel>(newIllness);
            return CreatedAtAction(nameof(GetIllnessById), new { id = createdIllnessRespondModel.IllnessId }, createdIllnessRespondModel);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateIllness(int id, [FromBody] IllnessRequestModel illnessRequestModel)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }

            if (illnessRequestModel == null)
            {
                return BadRequest();
            }

            var existingIllness = _illnessService.GetIllnessById(id);
            if (existingIllness == null)
            {
                return NotFound();
            }

            // Update the existing illness with the new data
            existingIllness.IllnessName = illnessRequestModel.IllnessName;

            // Save the updated illness
            _illnessService.UpdateIllness(existingIllness);

            // Return the updated illness as a response
            var updatedIllnessRespondModel = _mapper.Map<IllnessRespondModel>(existingIllness);
            return Ok(updatedIllnessRespondModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteIllness(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var existingIllness = _illnessService.GetIllnessById(id);
            if (existingIllness == null)
            {
                return NotFound();
            }
            // Delete the illness
            _illnessService.DeleteIllness(id);
            return NoContent();
        }



    }
}





