using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistController : ControllerBase
    {
        private readonly IDentistService _dentistService;

        private readonly IMapper _mapper;

        public DentistController(IMapper mapper,
            IDentistService dentistService)
        {
            _mapper = mapper;
            _dentistService = dentistService;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDentistAccountById(int id)
        {
            try
            {
                var dentist = _dentistService.GetDentistById(id);
                if (dentist == null)
                {
                    throw new Exception("Dentist is not existed!!!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Dentist retrieved successfully",
                    Data = dentist
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Dentist is not existed",
                    Error = ex.Message
                };
                return NotFound(response);
            }


        }

        [HttpPost]
        public IActionResult CreateDentistAccount([FromBody] DentistAccountApiModel dentistAccount)
        {
            try
            {
                var dentist = _dentistService.CreateDentist(_mapper.Map<Dentist>(dentistAccount));
                var res = _mapper.Map<DentistApiModel>(dentist);
                return CreatedAtAction(nameof(GetDentistAccountById), new { id = res.DentistId }, res);
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

        [HttpPut("{dentistAccountId}")]
        public IActionResult UpdateDentistAccount([FromBody] DentistAccountApiModel dentistAccount, int dentistAccountId)
        {
            try
            {
                var dentist = _dentistService.UpdateDentist(dentistAccountId, _mapper.Map<Dentist>(dentistAccount));
                var res = _mapper.Map<DentistApiModel>(dentist);
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "Update successfully",
                    Data = res
                };
                return Ok(response);
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
    }
}
