using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DentistBooking.Infrastructure
{
    public partial class ProposeAppointment
    {
        public int ProposeAppointmentId { get; set; }
        public DateTime? Datetime { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Note { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [EnumDataType(typeof(ProposeAppointmentStatus))]
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public ProposeAppointmentStatus Status { get; set; }

        public Patient? Patient { get; set; }

        public int? PatientId { get; set; }

        public ProposeAppointment()
        {
            Status = ProposeAppointmentStatus.NotSeen; // Set the default value to NotSeen
        }
    }


    public enum ProposeAppointmentStatus
    {
        NotSeen,
        Seen
    }

}
