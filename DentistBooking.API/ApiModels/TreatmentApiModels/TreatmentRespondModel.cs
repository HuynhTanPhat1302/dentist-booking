
using System.ComponentModel.DataAnnotations;
using DentistBooking.Infrastructure;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class TreatmentRespondModel
        {

            public TreatmentRespondModel()
            {
                TreatmentName = "Default Treament Name";
                Price = 2;
                EstimatedTime = 0.5;
            }
            public int TreatmentId { get; set; }
            public string TreatmentName { get; set; }
            public decimal Price { get; set; } //dolars
            public double EstimatedTime { get; set; } //hours
            
        }
    }
}
