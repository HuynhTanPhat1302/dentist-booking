using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    [ValidAppointmentDetail]
    public partial class AppointmentDetailCreateModel
    {
        

        [Required]
        [ValidId]
        public int? AppointmentId { get; set; }

        [Required]
        [ValidId]
        public int? MedicalRecordId { get; set; }        
    }
}
