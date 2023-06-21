
using System.ComponentModel.DataAnnotations;
using DentistBooking.Infrastructure;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class TreatmentRequestModel
        {

            public TreatmentRequestModel()
            {
                TreatmentName = "Default Treament Name";
                Price = 2;
                EstimatedTime = 0.5;
            }
            
            [MinLength(5)]
            [StringLength(150)]
            public string TreatmentName { get; set; }

            [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
            public decimal Price { get; set; } //dolars

            [Range(0.1, 5, ErrorMessage = "Estimated time must be greater than 0.")]
            public double EstimatedTime { get; set; } //hours
        }
    }
}
