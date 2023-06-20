using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;


namespace DentistBooking.API.Controllers
{
    [Route("api/propose-appointments")]
    [ApiController]
    public class ProposeAppointmentController : ControllerBase
    {
        private readonly IProposeAppointmentService _proposeAppointmentService;
        private readonly IMapper _mapper;

        public ProposeAppointmentController(IProposeAppointmentService proposeAppointmentService, IMapper mapper)
        {
            _proposeAppointmentService = proposeAppointmentService;
            _mapper = mapper;
        }

        //get proposeAppointment by id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProposeAppointmentById(int id)
        {
            var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);
            if (proposeAppointment == null)
            {
                return NotFound();
            }
            return Ok(proposeAppointment);
        }

        [HttpGet]
        [Route("get-propose-appointment-by-name/{name}")]
        public IActionResult GetProposeAppointmentByName(string name)
        {
            var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentByNameAsync(name);
            if (proposeAppointment == null)
            {
                return NotFound();
            }
            return Ok(proposeAppointment);
        }

        //search-proposeAppointment (paging, sort alphabalet)
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<ProposeAppointmentRequestModel>>> SearchProposeAppointments(int pageSize, int pageNumber, string searchQuery)
        {
            // Validation parameter

            if (pageSize <= 0)
            {
                return BadRequest("Page size must be greater than zero.");
            }

            if (pageNumber <= 0)
            {
                return BadRequest("Page number must be greater than zero.");
            }

            if (string.IsNullOrEmpty(searchQuery))
            {
                return BadRequest("seachQuery must not be null or empty.");
            }

            List<ProposeAppointment> proposeAppointments;
            proposeAppointments = await _proposeAppointmentService.SearchProposeAppointmentsAsync(pageSize, pageNumber, searchQuery);
            var proposeAppointmentApiRequestModels = _mapper.Map<List<ProposeAppointmentRequestModel>>(proposeAppointments);
            return proposeAppointmentApiRequestModels;
        }

        [HttpPost]
        public IActionResult CreateProposeAppointment(ProposeAppointmentRequestModel proposeAppointmentRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proposeAppointment = _mapper.Map<ProposeAppointment>(proposeAppointmentRequestModel);

            try
            {
                _proposeAppointmentService.CreateProposeAppointment(proposeAppointment);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during proposeAppointment creation
                return StatusCode(500, ex);
            }



            return CreatedAtAction(nameof(GetProposeAppointmentById), new { id = proposeAppointment.ProposeAppointmentId }, proposeAppointment);
        }

        // [HttpPut("{email}")]
        // public IActionResult UpdateProposeAppointment(string email, [FromBody] ProposeAppointmentApiRequestModel proposeAppointmentApiRequestModel)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentByEmail(email);
        //     if (proposeAppointment == null)
        //     {
        //         return NotFound();
        //     }

        //     // Use AutoMapper to map the properties from proposeAppointmentApiRequestModel to proposeAppointment
        //     _mapper.Map(proposeAppointmentApiRequestModel, proposeAppointment);

        //     try
        //     {
        //         _proposeAppointmentService.UpdateProposeAppointment(proposeAppointment);
        //     }
        //     catch (Exception ex)
        //     {
        //         // Handle any exceptions that occur during proposeAppointment update
        //         return StatusCode(500, ex);
        //     }

        //     return NoContent();
        // }

        // [HttpDelete("{email}")]
        // public async Task<IActionResult> DeleteProposeAppointment(string email)
        // {
        //     var proposeAppointment = await _proposeAppointmentService.GetProposeAppointmentByEmailAsync(email);
        //     if (proposeAppointment == null)
        //     {
        //         return NotFound();
        //     }

        //     try
        //     {
        //         if (proposeAppointment.Email != null)
        //         {
        //             await _proposeAppointmentService.DeleteProposeAppointmentAsync(proposeAppointment.Email);
        //         }
        //         else
        //         {
        //             return BadRequest("The ProposeAppointment have no email");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         // Handle any exceptions that occur during proposeAppointment deletion
        //         return StatusCode(500, ex);
        //     }

        //     return NoContent();
        // }
    }

}