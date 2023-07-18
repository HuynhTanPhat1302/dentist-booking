using System.ComponentModel.DataAnnotations;
using DentistBooking.API.Validation;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class StaffApiModel
        {
            [Required]
            [RegularExpression(@"^[a-zA-Z\s]+$")]
            public string? StaffName { get; set; }

            [Required]
            [RegularExpression("^P\\d{6}$")]
            public string? PhoneNumber { get; set; }
            
            [Required]
            [UniqueStaffEmail]
            public string? Email { get; set; }
        }
    }

}
