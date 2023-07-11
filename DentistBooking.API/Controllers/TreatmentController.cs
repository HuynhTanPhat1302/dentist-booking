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
    [Route("api/treatmentes")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentService _treatmentService;
        private readonly IMapper _mapper;

        public TreatmentController(ITreatmentService treatmentService, IMapper mapper)
        {
            _treatmentService = treatmentService;
            _mapper = mapper;
        }

        //get treatment by id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetTreatmentById(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var treatment = _treatmentService.GetTreatmentById(id);
            if (treatment == null)
            {
                return NotFound();
            }
            var treatmentRespondModel = _mapper.Map<TreatmentRespondModel>(treatment);
            return Ok(treatmentRespondModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetTreatmentes(int pageSize = 10, int pageNumber = 1)
        {
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

            var treatmentes = await _treatmentService.GetTreatmentesAsync(pageSize, pageNumber);
            if (treatmentes == null)
            {
                return NotFound();
            }
            var treatmentesRespondModel = _mapper.Map<List<TreatmentRespondModel>>(treatmentes);


            return Ok(treatmentesRespondModel);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<TreatmentRespondModel>>> SearchPatients(string searchQuery, int pageSize = 10, int pageNumber = 1)
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

            List<Treatment> treatmentes;
            if (string.IsNullOrEmpty(searchQuery))
            {
                treatmentes = await _treatmentService.GetTreatmentesAsync(pageSize, pageNumber);
            }
            else
            {
                treatmentes = await _treatmentService.SearchTreatmentsAsync(pageSize, pageNumber, searchQuery);
            }

            var treatmentesRespondModels = _mapper.Map<List<TreatmentRespondModel>>(treatmentes);
            return treatmentesRespondModels;
        }

        // HTTP POST - Create a new treatment
        [HttpPost]
        public IActionResult CreateTreatment([FromBody] TreatmentRequestModel treatmentRequestModel)
        {
            if (treatmentRequestModel == null)
            {
                return BadRequest();
            }

            // Map the create model to an Treatment entity
            var newTreatment = _mapper.Map<Treatment>(treatmentRequestModel);

            // Create the treatment
            _treatmentService.CreateTreatment(newTreatment);

            // Return the newly created treatment as a response
            var createdTreatmentRespondModel = _mapper.Map<TreatmentRespondModel>(newTreatment);
            return CreatedAtAction(nameof(GetTreatmentById), new { id = createdTreatmentRespondModel.TreatmentId }, createdTreatmentRespondModel);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateTreatment(int id, [FromBody] TreatmentRequestModel treatmentRequestModel)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }

            if (treatmentRequestModel == null)
            {
                return BadRequest();
            }

            var existingTreatment = _treatmentService.GetTreatmentById(id);
            if (existingTreatment == null)
            {
                return NotFound();
            }

            // Update the existing treatment with the new data
            
            _mapper.Map(treatmentRequestModel, existingTreatment);


            // Save the updated treatment
            _treatmentService.UpdateTreatment(existingTreatment);

            // Return the updated treatment as a response
            var updatedTreatmentRespondModel = _mapper.Map<TreatmentRespondModel>(existingTreatment);
            return Ok(updatedTreatmentRespondModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteTreatment(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var existingTreatment = _treatmentService.GetTreatmentById(id);
            if (existingTreatment == null)
            {
                return NotFound();
            }
            // Delete the treatment
            _treatmentService.DeleteTreatment(id);
            return NoContent();
        }
    }
}





