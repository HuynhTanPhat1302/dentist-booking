using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        private readonly IMapper _mapper;

        public DentistController(IMapper mapper,
            IAppointmentService appointmentService)
        {
            _mapper = mapper;
            _appointmentService = appointmentService;
        }

        [HttpGet("GetAppointmentsByDentisttEmail/{email}")]
        public IActionResult GetAppointmentsByDentisttEmail(string email)
        {
            var appointments = _appointmentService.GetAppointmentsByDentistEmail(email);

            if (appointments == null)
            {

                return NotFound();
            }

            var appointmentApiRequestModel = _mapper.Map<List<AppointmentApiRequestModel>>(appointments);


            return Ok(appointmentApiRequestModel);
        }
    }
}
