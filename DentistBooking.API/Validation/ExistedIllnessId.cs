using DentistBooking.Application.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class ExistedIllnessId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var illnessService = validationContext.GetService<IIllnessService>();

            if (illnessService != null && value != null)
            {
                int illnessId;
                if (Int32.TryParse(value.ToString(), out illnessId))
                {
                    if (!(illnessId >= 1 && illnessId <= int.MaxValue))
                    {
                        return new ValidationResult("id out of bound");

                    }
                    var exists = illnessService.GetIllnessById(illnessId);
                    if (exists == null)
                    {
                        return new ValidationResult("Illness ID does not exist in the database.");
                    }
                }
            }

            return ValidationResult.Success!;
        }
    }
}