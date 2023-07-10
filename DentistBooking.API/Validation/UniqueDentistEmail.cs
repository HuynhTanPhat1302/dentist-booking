using DentistBooking.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class UniqueDentistEmail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var dentistServie = validationContext.GetService<IDentistService>();

            if (dentistServie != null && value != null)
            {
                string? email = value.ToString();
                if (email != null)
                {
                    bool isUnique = dentistServie.IsEmailUnique(email);
                    if (isUnique == true)
                    {
                        return new ValidationResult("Email already exists. Please choose a different email.");
                    }
                }
            }
            return ValidationResult.Success!;
        }
    }
}
