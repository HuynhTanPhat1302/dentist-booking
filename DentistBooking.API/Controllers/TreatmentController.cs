using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public IActionResult GetAllTreatments()
        {
            var treamtments = _treatmentService.GetAllTreatments();
            return Ok(treamtments);
        }

        [HttpGet("{id}")]
        public IActionResult GetTreatmentById(int id)
        {
            var treatment = _treatmentService.GetTreatmentById(id);
            if (treatment == null)
            {
                return NotFound();
            }
            return Ok(treatment);
        }

        [HttpPost]
        public IActionResult CreateTreatment(TreatmentApiModel treatmentApiModel)
        {
            var treatment = _mapper.Map<Treatment>(treatmentApiModel);
            _treatmentService.CreateTreatment(treatment);
            return CreatedAtAction(nameof(GetTreatmentById), new { id = treatment.TreatmentId }, treatment);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateTreatment(int id, Treatment treatment)
        {
            if (id != treatment.TreatmentId)
            {
                return BadRequest();
            }
            _treatmentService.UpdateTreatment(treatment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTreatment(int id)
        {
            _treatmentService.DeleteTreatment(id);
            return NoContent();
        }
    }
}
