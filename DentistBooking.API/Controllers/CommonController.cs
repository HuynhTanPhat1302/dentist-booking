using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ITreatmentService _treatmentService;
        private readonly IProposeAppointmentService _proposeAppointmentService;
        private readonly IMapper _mapper;

        public CommonController(IAppointmentService appointmentService, IMapper mapper
            , ITreatmentService treatmentService,
            IProposeAppointmentService proposeAppointmentService)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _treatmentService = treatmentService;
            _proposeAppointmentService = proposeAppointmentService;
        }

        //get all treatments and its price
        

        [HttpGet("GetProposeAppointmentById/{id}")]
        public IActionResult GetProposeAppointmentById(int id)
        {
            var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);
            if (proposeAppointment == null)
            {
                return NotFound();
            }
            return Ok(proposeAppointment);
        }

        [HttpPost("CreateProposeAppointment")]
        public IActionResult CreateProposeAppointment(ProposeAppointmentRequestModel proposeAppointmentRequestModel)
        {
            var proposeAppointment = _mapper.Map<ProposeAppointment>(proposeAppointmentRequestModel);
            _proposeAppointmentService.CreateProposeAppointment(proposeAppointment);

            return CreatedAtAction(nameof(GetProposeAppointmentById), new { id = proposeAppointment.ProposeAppointmentId }, proposeAppointment);
        }
    }
}
