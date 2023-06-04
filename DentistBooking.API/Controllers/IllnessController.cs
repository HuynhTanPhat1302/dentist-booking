using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IllnessController : ControllerBase
    {
        private readonly IIllnessService _iillnessService;
        private readonly IMapper _mapper;

        public IllnessController(IIllnessService illnessService, IMapper mapper)
        {
            _iillnessService = illnessService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllIllness()
        {
            var illnesses = _iillnessService.GetAllIllnesss();
            return Ok(illnesses);
        }

        [HttpGet("{id}")]
        public IActionResult GetIllnessById(int id)
        {
            var illness = _iillnessService.GetIllnessById(id);
            if (illness == null)
            {
                return NotFound();
            }
            return Ok(illness);
        }

        [HttpPost]
        public IActionResult CreateIllness(IllnessApiModel illnessApiModel)
        {
            var illness = _mapper.Map<Illness>(illnessApiModel);
            _iillnessService.CreateIllness(illness);
            return CreatedAtAction(nameof(GetIllnessById), new { id = illness.IllnessId }, illness);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateIllness(int id, Illness illness)
        {
            if (id != illness.IllnessId)
            {
                return BadRequest();
            }   
            _iillnessService.UpdateIllness(illness);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteIllness(int id)
        {
            _iillnessService.DeleteIllness(id);
            return NoContent();
        }
    }
}
