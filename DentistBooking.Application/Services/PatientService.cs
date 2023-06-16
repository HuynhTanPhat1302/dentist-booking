﻿using DentistBooking.Application.Interfaces;
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




    }
}
