using DentistBooking.Application.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class ExistedmedicalRecordId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var illnessService = validationContext.GetService<IMedicalRecordService>();

            if (illnessService != null && value != null)
            {
                int illnessId;
                if (Int32.TryParse(value.ToString(), out illnessId))
                {
                    if (!(illnessId >= 1 && illnessId <= int.MaxValue))
                    {
                        return new ValidationResult("id out of bound");

                    }
                    var exists = illnessService.GetMedicalRecordById(illnessId);
                    if (exists == null)
                    {
                        return new ValidationResult("medicalRecord ID does not exist in the database.");
                    }
                }
            }

            return ValidationResult.Success!;
        }
    }
}