using DentistBooking.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class TreatmentDurationIsExistedOrNot : ValidationAttribute
    {
        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var treatmentService = validationContext.GetService<ITreatmentService>();

            if (treatmentService != null && value != null)
            {
                var treatmentTime = Double.Parse(value.ToString());
                if (treatmentTime != null)
                {
                    var isExisted = treatmentService.GetTreatmentByEstimatedTime(treatmentTime);
                    if (isExisted == null)
                    {
                        return new ValidationResult("Treatment is not existed already exists. Please choose a different treatment's estimated time.");
                    }
                }
            }
            return ValidationResult.Success!;
        }
    }
}
