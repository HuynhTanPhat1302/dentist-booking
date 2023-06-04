using AutoMapper;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
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
        private readonly IMapper _mapper;

        public CommonController(IAppointmentService appointmentService, IMapper mapper
            , ITreatmentService treatmentService)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _treatmentService = treatmentService;

        }

        //get all treatments and its price
        [HttpGet("GetTreatments")]
        public IActionResult GetAllTreatments()
        {
            var treatments = _treatmentService.GetAllTreatments();
            var treatmentApiRequestModel = _mapper.Map<List<TreatmentApiRequestModel>>(treatments);
            return Ok(treatmentApiRequestModel);
        }
    }
}
