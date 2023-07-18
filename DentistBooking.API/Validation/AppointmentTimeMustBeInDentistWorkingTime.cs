using DentistBooking.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class AppointmentTimeMustBeInDentistWorkingTime : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            DateTime date = DateTime.Parse(value.ToString());
            if (date != null)
            {
                var dentistAvailability = validationContext.GetService<IDentistAvailabilityService>();
                bool res = dentistAvailability.CheckEstimatedTimeIsExistedInFreeTiemAvailability(date).Result;
                if(res == false)
                {
                    return new ValidationResult("The estimated Time must be in dentist availability time");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
