using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DentistBooking.API.Validation;


namespace DentistBooking.API.ApiModels
{
    public class AppointmentUpdateModel
    {
        [Required]
        [ValidId]
        [PatientIdIsNotExisted]
        public int? PatientId { get; set; }

        [Required]
        [ValidId]
        [DentistIdIsExisted]
        public int? DentistId { get; set; }

        [Required]
        [ValidId]
        [UniqueStaffId]
        public int? StaffId { get; set; }

        [Required]
        [AppointmentTimeMustBeInDentistWorkingTime]
        public DateTime? Datetime { get; set; }

        [Required]
        [StatusOfAppointment]
        public string? Status { get; set; }


    }
}
