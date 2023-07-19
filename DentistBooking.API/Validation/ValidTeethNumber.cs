using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class ValidTeethNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int teethNumber;

                // Try to parse the value to an integer
                if (int.TryParse(value.ToString(), out teethNumber))
                {
                    // Check if the teeth number is between 1 and 32
                    if (teethNumber < 1 || teethNumber > 32)
                    {
                        return new ValidationResult("The teeth number must be between 1 and 32.");
                    }
                }
                else
                {
                    return new ValidationResult("The value must be an integer.");
                }
            }

            return ValidationResult.Success;
        }
    }
}