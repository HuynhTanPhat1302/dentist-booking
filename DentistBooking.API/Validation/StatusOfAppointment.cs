using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class StatusOfAppointmentAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string? status = value.ToString();
                if (status != "Scheduled" && status != "Finished" && status != "Canceled")
                {
                    return new ValidationResult("The status must be either 'Finished' or 'Scheduled' or 'Canceled'.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
