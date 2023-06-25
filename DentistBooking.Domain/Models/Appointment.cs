using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DentistBooking.Infrastructure
{
    public partial class Appointment
    {
        public Appointment()
        {
            AppointmentDetails = new HashSet<AppointmentDetail>();
            Status = AppointmentStatus.NotYet;
        }

        public int AppointmentId { get; set; }
        public int? PatientId { get; set; }
        public int? DentistId { get; set; }
        public int? StaffId { get; set; }
        public DateTime? Datetime { get; set; }
        public double? Duration { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        [EnumDataType(typeof(ProposeAppointmentStatus))]
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public AppointmentStatus Status { get; set; }

        public virtual Dentist? Dentist { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual staff? Staff { get; set; }
        public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }
    }

    public enum AppointmentStatus
    {
        NotYet,
        Finished,
        Canceled
    }
}
