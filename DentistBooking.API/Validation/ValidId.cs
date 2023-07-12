using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class ValidIdAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) {
                return ValidationResult.Success;
            }
            if (value != null)
            {
                int id;
                if (int.TryParse(value.ToString(), out id))
                {
                    if (id >= 1 && id <= int.MaxValue)
                    {
                        return ValidationResult.Success;
                    }
                }
            }

            return new ValidationResult("The ID must be a valid integer between 1 and " + int.MaxValue);
        }
    }
}
