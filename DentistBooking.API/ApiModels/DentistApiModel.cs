﻿using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    public class DentistApiModel
    {
        public int DentistId { get; set; }

        public string? Email { get; set; }

        public string? DentistName { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
