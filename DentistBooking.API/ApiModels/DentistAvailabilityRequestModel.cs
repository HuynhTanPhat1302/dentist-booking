﻿using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public class DentistAvailabilityRequestModel
    {
        [Required]
        public int DentistId { get; set; }
        [Required]
        public string? DayOfWeek { get; set; }
        [Required]
        public string? StartTime { get; set; }
        [Required]
        public string? EndTime { get; set; }
    }
}
