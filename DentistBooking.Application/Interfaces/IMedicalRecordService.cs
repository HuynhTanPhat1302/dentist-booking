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
        MedicalRecord GetMedicalRecordById(int id);
        void CreateMedicalRecord(MedicalRecord medicalRecord);
        void UpdateMedicalRecord(MedicalRecord medicalRecord);
        void DeleteMedicalRecord(int id);
        List<MedicalRecord> GetMedicalRecordsByPatientEmail(string email);
        MedicalRecord? GetMedicalRecords(int id);
        List<MedicalRecord> GetMedicalRecordsOfPatient(int patientId);

    }
}
