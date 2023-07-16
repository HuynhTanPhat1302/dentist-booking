using DentistBooking.API.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DentistBooking.API.ApiModels
{
    public class ProposeAppointmentUpdateModel
    {
        
        [NotPastOrPresentDate]
        //[RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}Z$")]
        [DataType(DataType.DateTime)]
        [NotMoreThanOneMonth]
        public DateTime? Datetime { get; set; }

       
        [StringLength(150)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string? Name { get; set; }

     
        [StringLength(20)]
        [RegularExpression(@"^\d{10}$")]
        public string? PhoneNumber { get; set; }


        [StringLength(int.MaxValue)]
        public string? Note { get; set; }

        [ValidId]
        public int? PatientId { get; set; }
    }
}


