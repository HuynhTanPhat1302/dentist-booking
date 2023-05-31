using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IStaffService
    {
         Appointment CreateAnAppointment(Appointment appointment);
         Patient CreateAccountOfPatient(Patient patient);
         List<Patient> GetAllPatients();
         Patient GetPatient(int id);
         List<MedicalRecord> GetMedicalRecords(int id);
    }
}
