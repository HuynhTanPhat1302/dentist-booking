using DentistBooking.Application.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class ExistedTreatmentId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var treatmentService = validationContext.GetService<ITreatmentService>();

            if (treatmentService != null && value != null)
            {
                int treatmentId;
                if (Int32.TryParse(value.ToString(), out treatmentId))
                {
                    if (!(treatmentId >= 1 && treatmentId <= int.MaxValue))
                    {
                        return new ValidationResult("id out of bound");

                    }
                    var exists = treatmentService.GetTreatmentById(treatmentId);
                    if (exists == null)
                    {
                        return new ValidationResult("Treatment ID does not exist in the database.");
                    }
                }
            }

            return ValidationResult.Success!;
        }
    }
}