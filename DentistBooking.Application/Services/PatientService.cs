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

        public Patient? GetPatientByEmail(string email)
        {
            return _patientRepository.GetPatientfByEmail(email);

        }

        //public async Task<List<Patient>> SearchPatientsAsync(int pageSize, int pageNumber, string searchQuery = "")
        //{
        //    var patients = await _patientRepository.SearchPatientsAsync(searchQuery);

        //    var pagedPatients = patients
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    return pagedPatients;
        //}

        //public async Task<List<Patient>> SearchPatientsAsync(int pageSize, int pageNumber, string searchQuery = "")
        //{
        //    List<Patient> patients;

        //    if (string.IsNullOrEmpty(searchQuery))
        //    {
        //        patients = await _patientRepository.GetAllPatientsAsync();
        //    }
        //    else
        //    {
        //        patients = await _patientRepository.SearchPatientsAsync(searchQuery);
        //    }

        //    var pagedPatients = patients
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    return pagedPatients;
        //}
        public async Task<List<Patient>> GetPatientsAsync(int pageSize, int pageNumber)
        {
            var patients = await _patientRepository.GetPatientsAsync();

            var pagedPatients = patients
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedPatients;
        }

        public async Task<List<Patient>> SearchPatientsAsync(int pageSize, int pageNumber, string searchQuery)
        {
            var patients = await _patientRepository.SearchPatientsAsync(searchQuery);

            var pagedPatients = patients
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedPatients;
        }

        public Patient? CreateAccountOfPatient(Patient patient)
        {
            var emailIsExisted = _patientRepository.GetAll().Where(p => p.Email.Equals(patient.Email)).FirstOrDefault();
            if (emailIsExisted != null)
            {
                throw new Exception("Email is existed");
            }

            var patientCodeIsExisted = _patientRepository.GetAll().Where(p => p.PatientCode.Equals(patient.PatientCode)).FirstOrDefault();
            if (patientCodeIsExisted != null)
            {
                throw new Exception("Patient code is existed");
            }
            _patientRepository.Add(patient);
            _patientRepository.SaveChanges();
            return patient;
        }

       
    }
}
