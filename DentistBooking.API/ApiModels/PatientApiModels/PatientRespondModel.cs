using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class PatientRespondModel
        {
            public int PatientId { get; set; }

            public string? Email { get; set; }

            public string? PatientName { get; set; }

            public string? PhoneNumber { get; set; }

            public DateTime? DateOfBirth { get; set; }

            public string? PatientCode { get; set; }

            public string? Address { get; set; }
        }
    }

}
