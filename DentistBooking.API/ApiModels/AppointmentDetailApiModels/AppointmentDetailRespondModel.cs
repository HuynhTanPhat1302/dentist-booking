using System;
using System.Collections.Generic;

namespace DentistBooking.API.ApiModels
{
    public partial class AppointmentDetailRespondModel
    {
        public int AppointmentDetailId { get; set; }
        public int? AppointmentId { get; set; }
        public int? MedicalRecordId { get; set; }        
    }
}
