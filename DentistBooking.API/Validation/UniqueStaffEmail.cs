using DentistBooking.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace DentistBooking.API.Validation
{
    public class UniqueStaffEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var staffService = validationContext.GetService<IStaffService>();

            if (staffService != null && value != null)
            {
                string? email = value.ToString();
                if (email != null)
                {
                    bool isUnique = staffService.IsEmailUnique(email).Result;
                    if (!isUnique)
                    {
                        return new ValidationResult("Email already exists. Please choose a different email.");
                    }
                }
            }
            return ValidationResult.Success!;
        }
    }
}
