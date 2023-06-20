using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                if (id <= 0 || id > int.MaxValue)
                {
                    throw new Exception("The iD is out of bound");
                }
                return _patientRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
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

        public Patient UpdatePatient(int id, Patient patient)
        {
            var existedPatient = _patientRepository.GetById(id);
            if (existedPatient == null)
            {
                throw new Exception("patient is not existed");
            }

            var patientEmailIsExisted = _patientRepository.GetByEmail(existedPatient.Email);
            if (patientEmailIsExisted != null)
            {
                throw new Exception("Patient email is existed");
            }

            var paitentCodeIsExisted = _patientRepository.GetPatientByPatientCode(patient.PatientCode);
            if(paitentCodeIsExisted != null)
            {
                throw new Exception("Patient code is already existed");
            }
            existedPatient.Email = patient.Email;
            existedPatient.PatientCode = patient.PatientCode;
            existedPatient.PhoneNumber = patient.PhoneNumber;
            existedPatient.DateOfBirth = patient.DateOfBirth;
            existedPatient.PatientName = patient.PatientName;
            existedPatient.Address = patient.Address;
            _patientRepository.Update(patient);
            _patientRepository.SaveChanges();
            return existedPatient;
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

        public async Task DeletePatientAsync(string email)
        {
            var patient = await _patientRepository.GetPatientByEmailAsync(email);
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

        public async Task<Patient?> GetPatientByEmailAsync(string email)
        {
            return await _patientRepository.GetPatientByEmailAsync(email);

        }

        
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

        //check duplicated email
        public async Task<bool> IsEmailUnique(string email)
        {
            bool isUnique = false;

            if (!string.IsNullOrEmpty(email))
            {
                isUnique = await _patientRepository.IsEmailUnique(email);
            }

            return isUnique;
        }

        public async Task<bool> IsPatientCodeUnique(string patientCode)
        {
            bool isUnique = false;

            if (!string.IsNullOrEmpty(patientCode))
            {
                isUnique = await _patientRepository.IsEmailPatientCode(patientCode);
            }

            return isUnique;
        }

       
    }
}
