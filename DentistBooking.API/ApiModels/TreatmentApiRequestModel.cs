using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class TreatmentApiRequestModel
        {
            [Required]
            public string? TreatmentName { get; set; }

            [Required]
            public decimal? Price { get; set; }

            [Required]
            public double? EstimatedTime { get; set; }

        }
    }

}
