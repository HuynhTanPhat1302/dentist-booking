using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    public class ProposeAppointmentStatusRequestModel
    {
        [StatusOfProposeAppointment]
        [Required]
        public string? Status { get; set; }
    }
}


