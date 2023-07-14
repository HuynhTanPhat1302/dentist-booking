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
        [ValidId]
        public int? PatientId { get; set; }

        [Required]
        [ValidId]
        public int? DentistId { get; set; }

        [Required]
        [ValidId]
        public int? StaffId { get; set; }

        [Required]
        [NotMoreThanOneMonth]
        public DateTime? Datetime { get; set; }

        // [Required]
        [TreatmentDurationIsExistedOrNot]
        private double? Duration { get; set; }
        
    
    }
}
