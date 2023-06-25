using DentistBooking.API.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DentistBooking.API.ApiModels
{
    public class ProposeAppointmentRespondModel
    {
        public int ProposeAppointmentId { get; set; }

        
        public DateTime? Datetime { get; set; }


        
        public string? Name { get; set; }

       
        public string? PhoneNumber { get; set; }


    
        public string? Note { get; set; }

        public string? Status { get; set; }

        public int? PatientId  { get; set; }
    }
}


