using DentistBooking.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace DentistBooking.API.Validation
{
    public class UniquePatientCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var patientService = validationContext.GetService<IPatientService>();

            if (patientService != null && value != null)
            {
                string? patientCode = value.ToString();
                if (patientCode != null)
                {
                    bool isUnique = patientService.IsPatientCodeUnique(patientCode).Result;
                    if (!isUnique)
                    {
                        return new ValidationResult("Patient code already exists. Please choose a different Patient code.");
                    }
                }
            }
            return ValidationResult.Success!;
        }
    }
}
