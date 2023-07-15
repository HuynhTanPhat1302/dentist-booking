using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class StatusOfMedicalRecordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string? status = value.ToString();
                if (status != "Finished" && status != "ReExamination")
                {
                    return new ValidationResult("The status must be either 'Finished' or 'ReExamination'.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
