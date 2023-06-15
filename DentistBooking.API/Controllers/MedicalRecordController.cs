using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DentistBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IMapper _mapper;

        public MedicalRecordController(IMedicalRecordService medicalRecordService, IMapper mapper)
        {
            _medicalRecordService = medicalRecordService;
            _mapper = mapper;
        }
        /*// GET: api/<MedicalRecordController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MedicalRecordController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MedicalRecordController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MedicalRecordController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MedicalRecordController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/

        [HttpGet("{id}")]
        public IActionResult GetMedicalRecords(int id)
        {
            try
            {
                var medicalRecords = _medicalRecordService.GetMedicalRecords(id);
                var medicalRecordsDTO = _mapper.Map<MedicalRecordApiRequestModel>(medicalRecords);
                if (medicalRecords == null)
                {
                    throw new Exception("Patient is not existed!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "MedicalRecords retrieved successfully",
                    Data = medicalRecordsDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Medical Records is not existed!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }

        [HttpGet("/api/MedicalRecord-Of-Patient/{patientId}")]
        public IActionResult GetMedicalRecordsOfPatient(int patientId)
        {
            try
            {
                var medicalRecords = _medicalRecordService.GetMedicalRecordsOfPatient(patientId);
                var medicalRecordsDTO = _mapper.Map<List<MedicalRecordApiRequestModel>>(medicalRecords);
                if (medicalRecords.Count == 0)
                {
                    throw new Exception("Patient is not existed!");
                }
                var response = new
                {
                    ContentType = "application/json",
                    Success = true,
                    Message = "MedicalRecords retrieved successfully",
                    Data = medicalRecordsDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    ContentType = "application/json",
                    Success = false,
                    Message = "Patient is not existed!!!",
                    Error = ex.Message
                };
                return NotFound(response);
            }
        }
    }
}
