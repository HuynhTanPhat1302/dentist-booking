using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    public class MedicalRecordtStatusRequestModel
    {
        [StatusOfMedicalRecord]
        [Required]
        public string? Status { get; set; }
    }
}


