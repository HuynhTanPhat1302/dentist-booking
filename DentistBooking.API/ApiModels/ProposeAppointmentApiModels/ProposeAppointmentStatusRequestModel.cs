using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    public class ProposeAppointmentStatusRequestModel
    {
        [StatusOfProposeAppointment]
        public string? Status { get; set; }
    }
}


