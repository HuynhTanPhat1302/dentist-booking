using System.ComponentModel.DataAnnotations;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;

namespace DentistBooking.API.ApiModels
{
    public class AppointmentRespondModel
    {
        public int AppointmentId { get; set; }
        public int? PatientId { get; set; }
        public int? DentistId { get; set; }
        public int? StaffId { get; set; }
        public DateTime? Datetime { get; set; }
        public double? Duration { get; set; }
        public string? Status { get; set; }
    }
}
