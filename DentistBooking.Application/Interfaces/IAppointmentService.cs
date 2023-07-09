using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> GetAllAppointments();
        Appointment? GetAppointmentById(int id);

        Task<List<Appointment>> GetAllAppointmentsAsync();

        Task<List<Appointment>> GetAppointmentsAsync(int pageSize, int pageNumber);

        Task<List<Appointment>> GetAppointmentsByDentistIdAsync(int dentistId, int pageSize, int pageNumber);

        Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId, int pageSize, int pageNumber);




        Appointment? GetAppointmentByStaffId(int id);
        void CreateAppointment(Appointment appointment);
        Appointment UpdateAppointment(Appointment appointment);
        void DeleteAppointment(int id);
        List<Appointment> GetAppointmentsByPatientEmail(string email);

        List<Appointment> GetAppointmentsByDentistEmail(string dentistEmail);

        Appointment? CreateAnAppointment(Appointment appointment);


    }
}
