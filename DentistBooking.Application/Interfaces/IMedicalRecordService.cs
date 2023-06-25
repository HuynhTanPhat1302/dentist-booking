using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IMedicalRecordService
    {
        IEnumerable<MedicalRecord> GetAllMedicalRecords();

        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsAsync(int pageSize, int pageNumber);
        
        void CreateMedicalRecord(MedicalRecord medicalRecord);
        void UpdateMedicalRecord(MedicalRecord medicalRecord);
        void DeleteMedicalRecord(int id);
        List<MedicalRecord> GetMedicalRecordsByPatientEmail(string email);
        MedicalRecord? GetMedicalRecordById(int id);
        List<MedicalRecord> GetMedicalRecordsOfPatient(int patientId);

    }
}
