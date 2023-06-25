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

        public virtual DentistRepondModel? Dentist { get; set; }
        public virtual PatientRespondModel? Patient { get; set; }
        public virtual StaffRespondModel? Staff { get; set; }
        public virtual ICollection<AppointmentDetailRespondModel>? AppointmentDetails { get; set; }
    }
}
