using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public partial class AppointmentDetailCreateModel
    {
        

        [Required]
        public int? AppointmentId { get; set; }

        [Required]
        public int? MedicalRecordId { get; set; }        
    }
}
