using DentistBooking.Application.Utils;
using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IDentistService
    {
        List<TimeSlot> GetAvailableTimeSlots(DentistAvailability availability, List<Appointment> appointments, DateTime date);
    }
}
