using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class StatusOfProposeAppointmentAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string? status = value.ToString();
                if (status != "Seen" && status != "NotSeen")
                {
                    return new ValidationResult("The status must be either 'Seen' or 'NotSeen'.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
