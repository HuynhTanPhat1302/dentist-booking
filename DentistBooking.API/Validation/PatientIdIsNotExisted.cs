using DentistBooking.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class PatientIdIsNotExisted : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var patientService = validationContext.GetService<IPatientService>();

            if (patientService != null && value != null)
            {
                int patientId;
                if (Int32.TryParse(value.ToString(), out patientId))
                {
                    if (!(patientId >= 1 && patientId <= int.MaxValue))
                    {
                        return new ValidationResult("id out of bound");

                    }
                    var exists = patientService.GetPatientById(patientId);
                    if (exists == null)
                    {
                        return new ValidationResult("Patient ID does not exist in the database.");
                    }
                }
            }

            return ValidationResult.Success!;
        }
    }
}
