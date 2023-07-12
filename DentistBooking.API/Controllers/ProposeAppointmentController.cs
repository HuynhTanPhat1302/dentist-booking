using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


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
        //[AllowAnonymous]
        [Authorize(Policy = "StaffOnly")]
        public IActionResult GetProposeAppointmentById(int id)
        {
            var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);
            if (proposeAppointment == null)
            {
                return NotFound();
            }
            var proposeAppointmentRespondModel = _mapper.Map<ProposeAppointmentRespondModel>(proposeAppointment);
            return Ok(proposeAppointmentRespondModel);
        }

        //search-proposeAppointment (paging, sort alphabalet)
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<ProposeAppointmentRequestModel>>> SearchProposeAppointments(string searchQuery, int pageSize = 10, int pageNumber = 1)
        {
            // Validation parameter

            if (pageSize <= 0 && pageSize <= 200)
            {
                return BadRequest("Page size must be greater than zero and smaller 201");
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
        [AllowAnonymous]
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

            var createdProposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(proposeAppointment.ProposeAppointmentId);
            var proposeAppointmentRespondModel = _mapper.Map<ProposeAppointmentRespondModel>(createdProposeAppointment);


            return CreatedAtAction(nameof(GetProposeAppointmentById), new { id = proposeAppointment.ProposeAppointmentId }, proposeAppointmentRespondModel);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProposeAppointment(int id, [FromBody] ProposeAppointmentRequestModel proposeAppointmentRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);
            if (proposeAppointment == null)
            {
                return NotFound();
            }

            // Use AutoMapper to map the properties from proposeAppointmentApiRequestModel to proposeAppointment
            _mapper.Map(proposeAppointmentRequestModel, proposeAppointment);

            try
            {
                _proposeAppointmentService.UpdateProposeAppointment(proposeAppointment);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during proposeAppointment update
                return StatusCode(500, ex);
            }

            // Retrieve the updated propose appointment from the service
            var updatedProposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);

            //mapper
            var proposeAppointmentRespondModel = _mapper.Map<ProposeAppointmentRespondModel>(updatedProposeAppointment);

            // Return the updated propose appointment in the response
            return Ok(proposeAppointmentRespondModel);
        }

        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> GetProposeAppointmentsByStatus(string status, int pageSize = 10, int pageNumber = 1)
        {

            var proposeAppointments = await _proposeAppointmentService.GetProposeAppointmentsByStatusAsync(status, pageSize, pageNumber);

            var respondModels = _mapper.Map<List<ProposeAppointmentRespondModel>>(proposeAppointments);

            return Ok(respondModels);

        }


        [HttpPatch("{id}/status")]
        public IActionResult ChangeProposeAppointmentStatus(int id, [FromBody] ProposeAppointmentStatusRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);
            if (proposeAppointment == null)
            {
                return NotFound();
            }

            // Update the status of the propose appointment
            _mapper.Map(requestModel, proposeAppointment);


            try
            {
                _proposeAppointmentService.UpdateProposeAppointment(proposeAppointment);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during propose appointment update
                return StatusCode(500, ex);
            }

            // Return the updated propose appointment in the response
            var updatedProposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);
            var respondModel = _mapper.Map<ProposeAppointmentRespondModel>(updatedProposeAppointment);
            return Ok(respondModel);
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteProposeAppointment(int id)
        {
            var proposeAppointment = _proposeAppointmentService.GetProposeAppointmentById(id);
            if (proposeAppointment == null)
            {
                return NotFound();
            }

            try
            {
                _proposeAppointmentService.DeleteProposeAppointment(id);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during proposeAppointment deletion
                return StatusCode(500, ex);
            }

            return NoContent();
        }

    }

}