using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistAvailabilityController : ControllerBase
    {
        private readonly IDentistAvailabilityService _avaiService;
        private readonly IMapper _mapper;

        public DentistAvailabilityController(IDentistAvailabilityService avaiService, IMapper mapper)
        {
            _avaiService = avaiService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllDentistAvailabilities()
        {
            var avais = _avaiService.GetAllDentistbvailabilities();
            return Ok(avais);
        }

        [HttpGet("{id}")]
        public IActionResult GetDentistAvailabilityById(int id)
        {
            var avai = _avaiService.GetDentistAvailabilityById(id);
            if (avai == null)
            {
                return NotFound();
            }
            return Ok(avai);
        }

        [HttpPost]
        public IActionResult CreateDentistAvailability(DentistAvailabilityApiModel avaiApiModel)
        {
            var avai = _mapper.Map<DentistAvailability>(avaiApiModel);
            _avaiService.CreateDentistAvailability(avai);
            return CreatedAtAction(nameof(GetDentistAvailabilityById), new { id = avai.AvailabilityId }, avai);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateDentistAvailability(int id, DentistAvailability dentistAvailability)
        {
            if (id != dentistAvailability.AvailabilityId)
            {
                return BadRequest();
            }
            _avaiService.UpdateDentistAvailability(dentistAvailability);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDentistAvailability(int id)
        {
            _avaiService.DeleteDentistAvailability(id);
            return NoContent();
        }
    }
}
