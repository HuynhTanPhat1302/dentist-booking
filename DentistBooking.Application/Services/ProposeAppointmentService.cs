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

        public async Task<List<ProposeAppointment>> GetProposeAppointmentsByStatusAsync(string status, int pageSize, int pageNumber)
        {
            try
            {
                var proposeAppointments = await _proposeAppointmentRepository.GetProposeAppointmentsByStatusAsync(status);

                // Apply paging
                var pagedProposeAppointments = proposeAppointments
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return pagedProposeAppointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public async Task<List<ProposeAppointment>> GetProposeAppointmentsByEmailAsync(string email)
        {
            try
            {
                var proposeAppointments = await _proposeAppointmentRepository.GetProposeAppointmentsByEmailAsync(email);

                return proposeAppointments;
            }
            catch (Exception ex)
            {
                // Log or handle the exception here as needed
                throw new Exception(ex.ToString());
            }

        }



        public async Task<List<ProposeAppointment>> SearchProposeAppointmentsAsync(int pageSize, int pageNumber, string searchQuery)
        {
            var proposeAppointments = await _proposeAppointmentRepository.SearchProposeAppointmentsAsync(searchQuery);

            var pagedProposeAppointments = proposeAppointments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedProposeAppointments;
        }

        public async Task<ProposeAppointment?> GetProposeAppointmentByNameAsync(string name)
        {
            return await _proposeAppointmentRepository.GetProposeAppointmentByNameAsync(name);
        }

        public void CreateProposeAppointment(ProposeAppointment proposeAppointment)
        {
            _proposeAppointmentRepository.Add(proposeAppointment);
            _proposeAppointmentRepository.SaveChanges();
        }

        public void UpdateProposeAppointment(ProposeAppointment proposeAppointment)
        {
            var existing = _proposeAppointmentRepository.GetByIdV2(proposeAppointment.ProposeAppointmentId);
            if (existing != null)
            {
                foreach (var propertyInfo in typeof(ProposeAppointment).GetProperties())
                {
                    var newValue = propertyInfo.GetValue(proposeAppointment);
                    if (newValue != null)
                    {
                        propertyInfo.SetValue(proposeAppointment, newValue);
                    }
                }
               
                
                _proposeAppointmentRepository.Update(existing);
                _proposeAppointmentRepository.SaveChanges();
            }
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
