using DentistBooking.Application.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class ExistedAppointmentId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var appointmentService = validationContext.GetService<IAppointmentService>();

            if (appointmentService != null && value != null)
            {
                int appointmentId;
                if (Int32.TryParse(value.ToString(), out appointmentId))
                {
                    if (!(appointmentId >= 1 && appointmentId <= int.MaxValue))
                    {
                        return new ValidationResult("id out of bound");

                    }
                    var exists = appointmentService.GetAppointmentById(appointmentId);
                    if (exists == null)
                    {
                        return new ValidationResult("Appointment ID does not exist in the database.");
                    }
                }
            }

            return ValidationResult.Success!;
        }
    }
}