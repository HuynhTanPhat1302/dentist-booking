using System;
using System.Collections.Generic;

namespace DentistBooking.API.ApiModels
{
    public class StaffRespondModel
    {
        public int StaffId { get; set; }
        public string? StaffName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
