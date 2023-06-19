using System.Text;
using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DentistBooking.API.Controllers
{
    [Route("api/illnesses")]
    [ApiController]
    public class IllnessController : ControllerBase
    {
        private readonly IIllnessService _illnessService;
        private readonly IMapper _mapper;

        public IllnessController(IIllnessService illnessService, IMapper mapper)
        {
            _illnessService = illnessService;
            _mapper = mapper;
        }

        //get illness by id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetIllnessById(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return BadRequest();
            }
            var illness = _illnessService.GetIllnessById(id);
            if (illness == null)
            {
                return NotFound();
            }
            return Ok(illness);
        }

        [HttpGet]
        public async Task<IActionResult> GetIllnesses(int pageSize = 10, int pageNumber = 1)
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
        
            var illnesses = await _illnessService.GetIllnessesAsync(pageSize, pageNumber);
            if (illnesses == null)
            {
                return NotFound();
            }
            var illnessesRespondModel = _mapper.Map<List<IllnessRespondModel>>(illnesses);


            return Ok(illnessesRespondModel);
        }

    //     [HttpGet]
    //     [Route("search")]
    //     public async Task<ActionResult<List<PatientApiRequestModel>>> SearchPatients(int pageSize, int pageNumber, string searchQuery)
    //     {
    //         // Validation parameter

    //         if (pageSize <= 0)
    //         {
    //             var response = new
    //             {
    //                 Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    //                 Title = "One or more validation errors occurred.",
    //                 Status = 400,
    //                 Errors = new Dictionary<string, List<string>>
    // {
    //     { "pageSize", new List<string> { "Page size must be greater than zero." } }
    // }
    //             };

    //             return BadRequest(response);
    //         }

    //         if (pageNumber <= 0)
    //         {
    //             var response = new
    //             {
    //                 Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    //                 Title = "One or more validation errors occurred.",
    //                 Status = 400,
    //                 Errors = new Dictionary<string, List<string>>
    // {
    //     { "pageNumber", new List<string> { "Page number must be greater than zero." } }
    // }
    //             };
    //             return BadRequest(response);
    //         }

    //         if (string.IsNullOrEmpty(searchQuery))
    //         {
    //             var response = new
    //             {
    //                 Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    //                 Title = "One or more validation errors occurred.",
    //                 Status = 400,
    //                 Errors = new Dictionary<string, List<string>>
    // {
    //     { "searchQuery", new List<string> { "seachQuery must not be null or empty." } }
    // }
    //             };
    //             return BadRequest(response);
    //         }

    //         List<Patient> patients;
    //         if (string.IsNullOrEmpty(searchQuery))
    //         {
    //             patients = await _patientService.GetPatientsAsync(pageSize, pageNumber);
    //         }
    //         else
    //         {
    //             patients = await _patientService.SearchPatientsAsync(pageSize, pageNumber, searchQuery);
    //         }

    //         var patientApiRequestModels = _mapper.Map<List<PatientApiRequestModel>>(patients);
    //         return patientApiRequestModels;
    //     }

    }


}





