using System.ComponentModel.DataAnnotations;
using DentistBooking.Infrastructure;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class IllnessRequestModel {
            public IllnessRequestModel()
            {
                IllnessName = "default name";
            }

            [MinLength(5)]
            [StringLength(150)]
            public string IllnessName { get; set; }

        }
    }
}
