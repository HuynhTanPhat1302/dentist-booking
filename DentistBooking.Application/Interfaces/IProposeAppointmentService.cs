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
        IEnumerable<ProposeAppointment> GetAllProposeAppointments();
        ProposeAppointment GetProposeAppointmentById(int id);
        void CreateProposeAppointment(ProposeAppointment ProposeAppointment);
        void UpdateProposeAppointment(ProposeAppointment ProposeAppointment);
        void DeleteProposeAppointment(int id);

        Task<ProposeAppointment?> GetProposeAppointmentByNameAsync(string name);

        Task<List<ProposeAppointment>> SearchProposeAppointmentsAsync(int pageSize, int pageNumber, string searchQuery);

        Task<List<ProposeAppointment>> GetProposeAppointmentsByStatusAsync(string status, int pageSize, int pageNumber);




        
    }
}
