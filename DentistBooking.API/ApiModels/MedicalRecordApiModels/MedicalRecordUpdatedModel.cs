using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public partial class MedicalRecordUpdatedModel
    {
        
        [Required]
        public int? PatientId { get; set; }

        [Required]
        public int? DentistId { get; set; }

        [Required]
        public int? TeethNumber { get; set; }

        [Required]
        public int? IllnessId { get; set; }

        [Required]
        public int? TreatmentId { get; set; }

        [Required]
        public string? Status { get; set; }
    }
}