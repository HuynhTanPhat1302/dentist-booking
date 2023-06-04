
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class NotMoreThanOneMonthAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTimeValue)
            {
                DateTime currentDate = DateTime.Now;
                DateTime maxAllowedDate = currentDate.AddMonths(1);

                if (dateTimeValue > maxAllowedDate)
                {
                    return new ValidationResult("The date should not be more than 1 month from the current date.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
