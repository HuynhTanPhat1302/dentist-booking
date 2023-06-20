using System.ComponentModel.DataAnnotations;
using DentistBooking.Infrastructure;

namespace DentistBooking.API.ApiModels
{
    namespace DentistBooking.API.ApiModels
    {
        public class IllnessRespondModel {
            public int IllnessId { get; set; }
            public string? IllnessName { get; set; }
            public virtual ICollection<MedicalRecordRespondModel>? MedicalRecords { get; set; }
        }
    }
}
