using System.ComponentModel.DataAnnotations;
namespace DentistBooking.API.ApiModels
{
    public class TreatmentApiModel
    {
        [Required]
        public string? TreatmentName { get; set; }          

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public double? EstimatedTime { get; set; }

    }
}
