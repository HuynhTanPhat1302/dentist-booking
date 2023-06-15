using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        // GET: api/<AppointmentConmtroller>
        /*[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/
        [HttpPost]
        public IActionResult CreateAnAppointment([FromBody] AppointmentApiModelRequest appointment)
        {
            try
            {
                var appointmentResponse = _appointmentService.CreateAnAppointment(_mapper.Map<Appointment>(appointment));
                if (appointmentResponse == null)
                {
                    throw new Exception("Booking time is existed");
                }
                var res = _mapper.Map<AppointmentApiModel>(appointmentResponse);
                return CreatedAtAction(nameof(GetAnAppointment), new { id = res.AppointmentId }, res);
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
            }

        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAnAppointment(int id)
        {
            try
            {
                var appointment = _appointmentService.GetAppointmentById(id);
                if (appointment == null)
                {
                    throw new Exception("Appointment is not existed!!!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Appointment retrieved successfully",
                    Data = _mapper.Map<AppointmentApiModel>(appointment)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Appointment is not existed!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }

        [HttpGet]
        public IActionResult GetAllBookingAppointment()
        {
            try
            {
                var res = _appointmentService.GetAllAppointments().ToList();
                if (res.Count == 0)
                {
                    throw new Exception("The list is empty!!!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Appointments retrieved successfully",
                    Data = _mapper.Map<List<AppointmentApiModel>>(res)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "The list is empty!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }
        /*// GET api/<AppointmentConmtroller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AppointmentConmtroller>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AppointmentConmtroller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AppointmentConmtroller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
