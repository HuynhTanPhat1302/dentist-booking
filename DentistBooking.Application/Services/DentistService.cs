using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Utils;
using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Services
{
    public class DentistService : IDentistService
    {
        public List<TimeSlot> GetAvailableTimeSlots(DentistAvailability availability, List<Appointment> appointments, DateTime date)
        {
            TimeSpan startOfDay = (TimeSpan)availability.StartTime;
            TimeSpan endOfDay = (TimeSpan)availability.EndTime;

            List<TimeSlot> availableTimeSlots = new List<TimeSlot>();

            // Generate time slots for the whole day
            TimeSlot fullDayTimeSlot = new TimeSlot
            {
                StartTime = startOfDay,
                EndTime = endOfDay
            };
            availableTimeSlots.Add(fullDayTimeSlot);

            // Remove occupied time slots based on appointments
            foreach (var appointment in appointments)
            {
                DateTime appointmentDate = appointment.Datetime.Value.Date;
                if (appointmentDate == date)
                {
                    TimeSpan appointmentStart = appointment.Datetime.Value.TimeOfDay;
                    TimeSpan appointmentEnd = appointmentStart + TimeSpan.FromMinutes(appointment.Duration.Value);

                    availableTimeSlots.RemoveAll(slot => SlotIntersectsAppointment(slot, appointmentStart, appointmentEnd));
                }
            }

            return availableTimeSlots;
        }

        private bool SlotIntersectsAppointment(TimeSlot slot, TimeSpan appointmentStart, TimeSpan appointmentEnd)
        {
            return slot.StartTime < appointmentEnd && slot.EndTime > appointmentStart;
        }
    }
}
