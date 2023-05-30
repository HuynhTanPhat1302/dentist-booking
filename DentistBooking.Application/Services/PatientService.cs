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
    public class PatientService : IPatientService
    {
        private readonly PatientRepository _patientRepository;

        public PatientService(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _patientRepository.GetAll();
        }

        public Patient GetPatientById(int id)
        {
            return _patientRepository.GetById(id);
        }

        public void CreatePatient(Patient patient)
        {
            _patientRepository.Add(patient);
            _patientRepository.SaveChanges();
        }

        public void UpdatePatient(Patient patient)
        {
            _patientRepository.Update(patient);
            _patientRepository.SaveChanges();
        }

        public void DeletePatient(int id)
        {
            var patient = _patientRepository.GetById(id);
            if (patient != null)
            {
                _patientRepository.Delete(patient);
                _patientRepository.SaveChanges();
            }
        }
    }
}
