using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentService(AppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return _appointmentRepository.GetAll();
        }

        public Appointment GetAppointmentById(int id)
        {
            return _appointmentRepository.GetById(id);
        }

        public void CreateAppointment(Appointment appointment)
        {
            _appointmentRepository.Add(appointment);
            _appointmentRepository.SaveChanges();
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _appointmentRepository.Update(appointment);
            _appointmentRepository.SaveChanges();
        }

        public void DeleteAppointment(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            if (appointment != null)
            {
                _appointmentRepository.Delete(appointment);
                _appointmentRepository.SaveChanges();
            }
        }

        public List<Appointment> GetAppointmentsByPatientEmail(string email)
        {
            return _appointmentRepository.GetAppointmentsByPatientEmail(email);
        }

        public List<Appointment> GetAppointmentsByDentistEmail(string dentistEmail)
        {
            return _appointmentRepository.GetAppointmentsByDentistEmail(dentistEmail);
        }
    }
}
