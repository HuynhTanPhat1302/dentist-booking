using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    public class AppointmentStatusRequestModel
    {
        [StatusOfAppointment]
        [Required]
        public string? Status { get; set; }
    }
}


