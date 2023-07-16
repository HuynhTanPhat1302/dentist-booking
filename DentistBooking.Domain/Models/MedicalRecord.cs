using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DentistBooking.Infrastructure
{
    public partial class MedicalRecord
    {
        public MedicalRecord()
        {
            AppointmentDetails = new HashSet<AppointmentDetail>();
            Status = MedicalRecordStatus.Finished;
        }

        public int MedicalRecordId { get; set; }
        public int? PatientId { get; set; }
        public int? DentistId { get; set; }
        public int? TeethNumber { get; set; }
        public int? IllnessId { get; set; }
        public int? TreatmentId { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [EnumDataType(typeof(MedicalRecordStatus))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MedicalRecordStatus Status { get; set; }

        public virtual Dentist? Dentist { get; set; }
        public virtual Illness? Illness { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual Treatment? Treatment { get; set; }
        public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }
    }

    public enum MedicalRecordStatus
    {
        Finished,
        ReExamination
    }
}
