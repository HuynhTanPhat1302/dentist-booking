namespace DentistBooking.API.ApiModels
{
    public class ProposeAppointmentApiModel
    {
        public int ProposeAppointmentId { get; set; }
        public DateTime? Datetime { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
    }
}
