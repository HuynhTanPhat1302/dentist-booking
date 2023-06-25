using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DentistBooking.API.ApiModels
{
    public class AppointmentUpdateModel
    {
        [Required]
        public int? PatientId { get; set; }

        [Required]
        public int? DentistId { get; set; }

        [Required]
        public int? StaffId { get; set; }

        [Required]
        public DateTime? Datetime { get; set; }

        [Required]

        public double? Duration { get; set; }
        [Required]

        public string? Status { get; set; }


    }
}
