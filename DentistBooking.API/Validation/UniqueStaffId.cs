using DentistBooking.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class UniqueStaffId : ValidationAttribute
    {

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var patientService = validationContext.GetService<IStaffService>();

            if (patientService != null && value != null)
            {
                int patientId;
                if (Int32.TryParse(value.ToString(), out patientId))
                {
                    if (!(patientId >= 1 && patientId <= int.MaxValue))
                    {
                        return new ValidationResult("id out of bound");

                    }
                    var exists = patientService.GetStaffById(patientId);
                    if (exists == null)
                    {
                        return new ValidationResult("Staff ID does not exist in the database.");
                    }
                }
            }

            return ValidationResult.Success!;
        }
    }
}
