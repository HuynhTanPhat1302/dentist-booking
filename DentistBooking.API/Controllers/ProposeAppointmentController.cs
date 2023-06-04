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
    public class ProposeAppointmentController : ControllerBase
    {
        private readonly IProposeAppointmentService _iproposeAppointmentService;
        private readonly IMapper _mapper;
        // GET: api/<ProposeAppointmentController>
        public ProposeAppointmentController(IProposeAppointmentService proposeAppointmentService, IMapper mapper)
        {
            _iproposeAppointmentService = proposeAppointmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProposeAppointments()
        {
            var proposeAppointments = _iproposeAppointmentService.GetProposeAppointments();
            return Ok(proposeAppointments);
        }

        [HttpGet("{id}")]
        public IActionResult GetProposeAppointmentById(int id)
        {
            var proposeAppointment = _iproposeAppointmentService.GetProposeAppointmentById(id);
            if (proposeAppointment == null)
            {
                return NotFound();
            }
            return Ok(proposeAppointment);
        }

        [HttpPost]
        public IActionResult CreateProposeAppointment(ProposeAppointmentApiModel proposeAppointmentApiModel)
        {
            var proposeAppointment = _mapper.Map<ProposeAppointment>(proposeAppointmentApiModel);
            _iproposeAppointmentService.CreateProposeAppointment(proposeAppointment);
            return CreatedAtAction(nameof(GetProposeAppointmentById), new { id = proposeAppointment.ProposeAppointmentId }, proposeAppointment);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateProposeAppointment(int id, ProposeAppointment proposeAppointment)
        {
            if (id != proposeAppointment.ProposeAppointmentId)
            {
                return BadRequest();
            }
            _iproposeAppointmentService.UpdateProposeAppointment(proposeAppointment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProposeAppointment(int id)
        {
            _iproposeAppointmentService.DeleteProposeAppointment(id);
            return NoContent();
        }
    }
}
