using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IProposeAppointmentService
    {
        IEnumerable<ProposeAppointment> GetProposeAppointments();
        ProposeAppointment GetProposeAppointmentById(int id);
        void CreateProposeAppointment(ProposeAppointment proposeAppointment);
        void UpdateProposeAppointment(ProposeAppointment proposeAppointment);
        void DeleteProposeAppointment(int id);
    }
}
