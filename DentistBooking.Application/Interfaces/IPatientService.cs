using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAllPatients();
        Patient GetPatientById(int id);
        void CreatePatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeletePatient(int id);

        Patient? GetPatientByEmail(string email);

        Task<List<Patient>> GetPatientsAsync(int pageSize, int pageNumber);

        Task<List<Patient>> SearchPatientsAsync(int pageSize, int pageNumber, string searchQuery);

        Patient? CreateAccountOfPatient(Patient patient);
    }
}
