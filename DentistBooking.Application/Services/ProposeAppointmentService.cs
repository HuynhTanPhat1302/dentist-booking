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
    public class ProposeAppointmentService : IProposeAppointmentService
    {
        private readonly ProposeAppointmentRepository _proposeAppointmentRepository;

        public ProposeAppointmentService(ProposeAppointmentRepository proposeAppointmentRepository)
        {
            _proposeAppointmentRepository = proposeAppointmentRepository;
        }

        public IEnumerable<ProposeAppointment> GetAllProposeAppointments()
        {
            return _proposeAppointmentRepository.GetAll();
        }

        public ProposeAppointment GetProposeAppointmentById(int id)
        {
            return _proposeAppointmentRepository.GetById(id);
        }

        public void CreateProposeAppointment(ProposeAppointment proposeAppointment)
        {
            _proposeAppointmentRepository.Add(proposeAppointment);
            _proposeAppointmentRepository.SaveChanges();
        }

        public void UpdateProposeAppointment(ProposeAppointment proposeAppointment)
        {
            _proposeAppointmentRepository.Update(proposeAppointment);
            _proposeAppointmentRepository.SaveChanges();
        }

        public void DeleteProposeAppointment(int id)
        {
            var proposeAppointment = _proposeAppointmentRepository.GetById(id);
            if (proposeAppointment != null)
            {
                _proposeAppointmentRepository.Delete(proposeAppointment);
                _proposeAppointmentRepository.SaveChanges();
            }
        }

        
    }
}
