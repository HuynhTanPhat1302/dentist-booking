using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DentistBooking.API.Controllers
{
    [Route("api/medical-records")]
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

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetMedicalRecordById(int id)
        {
            var medicalRecord = _medicalRecordService.GetMedicalRecordById(id);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            // You can map the MedicalRecord object to a MedicalRecordRespondModel using AutoMapper if needed
            var medicalRecordRespondModel = _mapper.Map<NestedMedicalRecordRespondModel>(medicalRecord);

            return Ok(medicalRecordRespondModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicalRecords(int pageSize = 10, int pageNumber = 1)
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

            var medicalRecords = await _medicalRecordService.GetMedicalRecordsAsync(pageSize, pageNumber);
            if (medicalRecords == null)
            {
                return NotFound();
            }
            var medicalRecordRespondModels = _mapper.Map<IEnumerable<NestedMedicalRecordRespondModel>>(medicalRecords);


            return Ok(medicalRecordRespondModels);
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

        
        // HTTP POST - Create a new illness
        [HttpPost]
        public IActionResult CreateMedicalRecord(MedicalRecordCreatedModel medicalRecordCreatedModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicalRecord = _mapper.Map<MedicalRecord>(medicalRecordCreatedModel);

            try
            {
                _medicalRecordService.CreateMedicalRecord(medicalRecord);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during medicalRecord creation
                return StatusCode(500, ex);
            }

            var createdMedicalRecord = _medicalRecordService.GetMedicalRecordById(medicalRecord.MedicalRecordId);
            var medicalRecordRespondModel = _mapper.Map<NestedMedicalRecordRespondModel>(createdMedicalRecord);


            return CreatedAtAction(nameof(GetMedicalRecordById), new { id = medicalRecord.MedicalRecordId }, medicalRecordRespondModel);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMedicalRecord(int id, [FromBody] MedicalRecordUpdatedModel medicalRecordUpdatedModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicalRecord = _medicalRecordService.GetMedicalRecordById(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            // Use AutoMapper to map the properties from medicalRecordApiRequestModel to medicalRecord
            _mapper.Map(medicalRecordUpdatedModel, medicalRecord);

            try
            {
                _medicalRecordService.UpdateMedicalRecord(medicalRecord);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during medicalRecord update
                return StatusCode(500, ex);
            }

            // Retrieve the updated propose appointment from the service
            var updatedMedicalRecord = _medicalRecordService.GetMedicalRecordById(id);

            //mapper
            var medicalRecordRespondModel = _mapper.Map<NestedMedicalRecordRespondModel>(updatedMedicalRecord);

            // Return the updated propose appointment in the response
            return Ok(medicalRecordRespondModel);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteMedicalRecord(int id)
        {
            var medicalRecord = _medicalRecordService.GetMedicalRecordById(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            try
            {
                _medicalRecordService.DeleteMedicalRecord(id);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during medicalRecord deletion
                return StatusCode(500, ex);
            }

            return NoContent();
        }


    }
}
