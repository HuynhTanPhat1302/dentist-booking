using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    public class AppointmentCreateModel
    {
        [Required]
        public int? PatientId { get; set; }

        [Required]
        public int? DentistId { get; set; }

        [Required]
        public int? StaffId { get; set; }

        [Required]
        [NotMoreThanOneMonth]
        public DateTime? Datetime { get; set; }

        [Required]
        [TreatmentDurationIsExistedOrNot]
        public double? Duration { get; set; }
        
        
    }
}
