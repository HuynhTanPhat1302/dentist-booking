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
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly MedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordService(MedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public IEnumerable<MedicalRecord> GetAllMedicalRecords()
        {
            return _medicalRecordRepository.GetAll();
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsAsync(int pageSize, int pageNumber) {
             try
            {
                var medicalRecords = await _medicalRecordRepository.GetMedicalRecordsAsync();

                var pagedMedicalRecords = medicalRecords
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return pagedMedicalRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }
        

        public void CreateMedicalRecord(MedicalRecord medicalRecord)
        {
            _medicalRecordRepository.Add(medicalRecord);
            _medicalRecordRepository.SaveChanges();
        }

        public void UpdateMedicalRecord(MedicalRecord medicalRecord)
        {
            _medicalRecordRepository.Update(medicalRecord);
            _medicalRecordRepository.SaveChanges();
        }

        public void DeleteMedicalRecord(int id)
        {
            var medicalRecord = _medicalRecordRepository.GetById(id);
            if (medicalRecord != null)
            {
                _medicalRecordRepository.Delete(medicalRecord);
                _medicalRecordRepository.SaveChanges();
            }
        }

        public List<MedicalRecord> GetMedicalRecordsByPatientEmail(string email)
        {
            return _medicalRecordRepository.GetMedicalRecordByPatientEmail(email);
        }

        public MedicalRecord? GetMedicalRecordById(int id)
        {
            return _medicalRecordRepository.GetMedicalRecordById(id);
        }

        public List<MedicalRecord> GetMedicalRecordsOfPatient(int patientId)
        {
            return _medicalRecordRepository.GetMedicalRecordByPatientId(patientId);
        }


    }
}
