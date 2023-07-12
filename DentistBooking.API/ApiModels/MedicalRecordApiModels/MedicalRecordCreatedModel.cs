using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    public partial class MedicalRecordCreatedModel
    {
        
        [Required]
        [ValidId]
        public int? PatientId { get; set; }

        [Required]
        [ValidId]
        public int? DentistId { get; set; }

        [Required]
        public int? TeethNumber { get; set; }

        [Required]
        [ValidId]
        public int? IllnessId { get; set; }

        [Required]
        [ValidId]
        public int? TreatmentId { get; set; }

        
    }
}
