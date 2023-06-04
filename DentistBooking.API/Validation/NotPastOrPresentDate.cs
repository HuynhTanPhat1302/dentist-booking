using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class NotPastOrPresentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTimeValue)
            {
                DateTime currentDate = DateTime.Now.Date;
                DateTime inputDate = dateTimeValue.Date;

                if (inputDate <= currentDate)
                {
                    return new ValidationResult("Date must be in the future and not today.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
