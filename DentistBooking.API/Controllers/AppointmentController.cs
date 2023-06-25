using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DentistBooking.API.Controllers
{
    [Route("api/appointments")]
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

        //get appointment by id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAppointmentById(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var appointment = _appointmentService.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            var appointmentRespondModel = _mapper.Map<AppointmentRespondModel>(appointment);
            return Ok(appointmentRespondModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentes(int pageSize = 10, int pageNumber = 1)
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

            var appointmentes = await _appointmentService.GetAppointmentsAsync(pageSize, pageNumber);
            if (appointmentes == null)
            {
                return NotFound();
            }
            var appointmentesRespondModel = _mapper.Map<List<AppointmentRespondModel>>(appointmentes);


            return Ok(appointmentesRespondModel);
        }



        // HTTP POST - Create a new appointment
        [HttpPost]
        public IActionResult CreateAppointment(AppointmentCreateModel appointmentCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = _mapper.Map<Appointment>(appointmentCreateModel);

            try
            {
                _appointmentService.CreateAppointment(appointment);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during proposeAppointment creation
                return StatusCode(500, ex);
            }

            var createdAppointment = _appointmentService.GetAppointmentById(appointment.AppointmentId);
            var appointmentRespondModel = _mapper.Map<AppointmentRespondModel>(createdAppointment);


            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.AppointmentId }, appointmentRespondModel);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateAppointment(int id, [FromBody] AppointmentUpdateModel appointmentUpdateModel)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }

            if (appointmentUpdateModel == null)
            {
                return BadRequest();
            }

            var existingAppointment = _appointmentService.GetAppointmentById(id);
            if (existingAppointment == null)
            {
                return NotFound();
            }

            // Update the existing appointment with the new data

            _mapper.Map(appointmentUpdateModel, existingAppointment);


            // Save the updated appointment
            _appointmentService.UpdateAppointment(existingAppointment);

            // Return the updated appointment as a response
            var updatedAppointmentRespondModel = _mapper.Map<AppointmentRespondModel>(existingAppointment);
            return Ok(updatedAppointmentRespondModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var existingAppointment = _appointmentService.GetAppointmentById(id);
            if (existingAppointment == null)
            {
                return NotFound();
            }
            // Delete the appointment
            _appointmentService.DeleteAppointment(id);
            return NoContent();
        }


        // [HttpPost]
        // public IActionResult CreateAnAppointment([FromBody] AppointmentApiModelRequest appointment)
        // {
        //     try
        //     {
        //         var appointmentResponse = _appointmentService.CreateAnAppointment(_mapper.Map<Appointment>(appointment));
        //         if (appointmentResponse == null)
        //         {
        //             throw new Exception("Booking time is existed");
        //         }
        //         var res = _mapper.Map<AppointmentApiModel>(appointmentResponse);
        //         return CreatedAtAction(nameof(GetAnAppointment), new { id = res.AppointmentId }, res);
        //     }
        //     catch (Exception ex)
        //     {
        //         var response = new
        //         {
        //             ContentType = "application/json",
        //             Success = false,
        //             Message = "Create unsuccesfully",
        //             Error = ex.Message
        //         };
        //         return BadRequest(response);
        //     }

        // }

        // [HttpGet]
        // [Route("{id}")]
        // public IActionResult GetAnAppointment(int id)
        // {
        //     try
        //     {
        //         var appointment = _appointmentService.GetAppointmentById(id);
        //         if (appointment == null)
        //         {
        //             throw new Exception("Appointment is not existed!!!");
        //         }
        //         var response = new
        //         {
        //             ContentType = "application/json",
        //             Success = true,
        //             Message = "Appointment retrieved successfully",
        //             Data = _mapper.Map<AppointmentApiModel>(appointment)
        //         };
        //         return Ok(response);
        //     }
        //     catch (Exception ex)
        //     {
        //         var response = new
        //         {
        //             ContentType = "application/json",
        //             Success = false,
        //             Message = "Appointment is not existed!!!",
        //             Error = ex.Message
        //         };
        //         return NotFound(response);
        //     }
        // }

        // [HttpGet]
        // public IActionResult GetAllBookingAppointment()
        // {
        //     try
        //     {
        //         var res = _appointmentService.GetAllAppointments().ToList();
        //         if (res.Count == 0)
        //         {
        //             throw new Exception("The list is empty!!!");
        //         }
        //         var response = new
        //         {
        //             ContentType = "application/json",
        //             Success = true,
        //             Message = "Appointments retrieved successfully",
        //             Data = _mapper.Map<List<AppointmentApiModel>>(res)
        //         };
        //         return Ok(response);
        //     }
        //     catch (Exception ex)
        //     {
        //         var response = new
        //         {
        //             ContentType = "application/json",
        //             Success = false,
        //             Message = "The list is empty!!!",
        //             Error = ex.Message
        //         };
        //         return NotFound(response);
        //     }
        // }

        // [HttpGet("/{email}")]
        // public IActionResult GetAppointmentsByDentisttEmail(string email)
        // {
        //     var appointments = _appointmentService.GetAppointmentsByDentistEmail(email);

        //     if (appointments == null)
        //     {

        //         return NotFound();
        //     }

        //     var appointmentApiRequestModel = _mapper.Map<List<AppointmentApiRequestModel>>(appointments);


        //     return Ok(appointmentApiRequestModel);
        // }

        //comment

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
