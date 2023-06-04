using DentistBooking.API.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DentistBooking.API.ApiModels
{
    public class ProposeAppointmentRequestModel
    {
        [Required]
        [NotPastOrPresentDate]
        //[RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}Z$")]
        [DataType(DataType.DateTime)]
        [NotMoreThanOneMonth]
        public DateTime? Datetime { get; set; }


        [Required]
        [StringLength(150)]
        public string? Name { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^\d{10}$")]
        public string? PhoneNumber { get; set; }

            [Required]
            [StringLength(int.MaxValue)]
            public string? Note { get; set; }
        }
}
