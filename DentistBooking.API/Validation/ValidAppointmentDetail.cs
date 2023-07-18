using DentistBooking.API.ApiModels;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.API.Validation
{
    public class ValidAppointmentDetail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (AppointmentDetailCreateModel)value;

            if (model.AppointmentId == null || model.MedicalRecordId == null)
            {
                return new ValidationResult("Both appointmentId and medicalRecordId are required.");
            }
            else
            {
                var dentistAvailabilityService = validationContext.GetService<IDentistAvailabilityService>();
                var appointmentService = validationContext.GetService<IAppointmentService>();
                var _appointmentRepository = validationContext.GetService<AppointmentRepository>();
                var medicalRecordService = validationContext.GetService<IMedicalRecordService>();
                var appointment = appointmentService.GetAppointmentById((int) model.AppointmentId);
                var medicalRecord = medicalRecordService.GetMedicalRecordById((int)model.MedicalRecordId);
                var timeStart = appointment.Datetime.Value.TimeOfDay;
                var timeEnd = appointment.Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double) medicalRecord.Treatment.EstimatedTime + (double) appointment.Duration));
                var listDentistAvailability = dentistAvailabilityService.GetDentistAvailabilitysByDayOfWeek(appointment.Datetime.Value,(int) appointment.DentistId);
                foreach(var item in listDentistAvailability)
                {
                    if (timeStart < item.StartTime || timeEnd > item.EndTime)
                    {
                        return new ValidationResult("Dentist is not working");
                    }
                }
                var listAppointmentOfDentis = _appointmentRepository.GetAll().Where(a => a.DentistId == appointment.DentistId && a.Datetime.Value.Date == appointment.Datetime.Value.Date).ToList();
                if (listAppointmentOfDentis.Count > 0)
                {
                    foreach (var dentistAppointment in listAppointmentOfDentis)
                    {
                        TimeSpan timeStartAppointment = dentistAppointment.Datetime.Value.TimeOfDay;
                        TimeSpan timeEndAppointment = timeStartAppointment.Add(TimeSpan.FromHours((double) dentistAppointment.Duration));
                        if (timeEnd >= timeStartAppointment && timeEnd <= timeEndAppointment)
                        {
                            return new ValidationResult("Dentist is busy now");
                        }
                    }
                }
            }

            // perform additional validation if necessary

            return ValidationResult.Success;
        }
    }
}
