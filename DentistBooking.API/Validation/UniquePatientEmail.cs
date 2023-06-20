using DentistBooking.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace DentistBooking.API.Validation
{
    public class UniquePatientEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var patientService = validationContext.GetService<IPatientService>();

            if (patientService != null && value != null)
            {
                string? email = value.ToString();
                if (email != null)
                {
                    bool isUnique = patientService.IsEmailUnique(email).Result;
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
