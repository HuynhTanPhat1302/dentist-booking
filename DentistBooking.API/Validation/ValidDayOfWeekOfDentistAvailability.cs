using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class ValidDayOfWeekOfDentistAvailability : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string? status = value.ToString();
                if (status != "Monday" && status != "Tuesday" && status != "Wednesday" && status != "Thursday" && status != "Friday" && status != "Saturday" && status != "Sunday")
                {
                    return new ValidationResult("The day of week must be either 'Monday' or 'Tuesday' or 'Wednesday' or 'Thursday' or 'Friday' or 'Saturday' or 'Sunday'.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
