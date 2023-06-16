using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Infrastructure.Repositories
{
    public class PatientRepository : RepositoryBase<Patient>
    {
        public PatientRepository(DentistBookingContext context) : base(context)
        {
        }

        //tanphat - async method - search patient with searchQuery
        public async Task<List<Patient>> SearchPatientsAsync(string searchQuery)
        {
            IQueryable<Patient> query = DbSet.Include(p => p.MedicalRecords).Include(p => p.Appointments);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.PatientName != null && p.PatientName.Contains(searchQuery));
            }

            var patients = await query.OrderBy(p => p.PatientName).ToListAsync();

            return patients;
        }


        //tanphat - get all patients
        public async Task<List<Patient>> GetPatientsAsync()
        {
            var patients = await DbSet
                .Include(p => p.MedicalRecords)
                .Include(p => p.Appointments)
                .OrderBy(p => p.PatientName)
                .ToListAsync();

            return patients;
        }

        //tanphat - get all patients
        public List<Patient> GetAllPatients()
        {
            return DbSet.Include(p => p.Appointments)
                .Include(p => p.MedicalRecords)
                .ToList();
        }

        public Patient? GetByEmail(string email)
        {
            return DbSet.Include(p => p.Appointments)
                .Include(p => p.MedicalRecords)
                .SingleOrDefault(p => p.Email == email);
        }

        public Patient? FindById(int id)
        {
            return DbSet.Include(p => p.Appointments)
                .Include(p => p.MedicalRecords)
                .SingleOrDefault(p => p.PatientId == id);
        }

        public Patient? GetPatientfByEmail(string email)
        {
            return DbSet.FirstOrDefault(s => s.Email == email);
        }

        public async Task<Patient?> GetPatientByEmailAsync(string email)
        {
            // Check if there is any patient with the provided email in the database
            var patient = await DbSet.FirstOrDefaultAsync(s => s.Email == email);
            return patient;
        }



        public async Task<bool> IsEmailUnique(string email)
        {
            // Check if there is any patient with the provided email in the database
            bool isUnique = !await DbSet.AnyAsync(p => p.Email == email);
            return isUnique;
        }
        // Add any additional custom methods or queries specific to the Patient entity here
    }
}
