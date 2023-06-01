using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Infrastructure.Repositories
{
    public class MedicalRepository : RepositoryBase<MedicalRecord>
    {
        public MedicalRepository(DentistBookingContext context) : base(context)
        {
        }

        public MedicalRecord? GetMedicalById(int id)
        {
            var medicalRecord = DbSet
            .Include(mr => mr.Dentist)
            .Include(mr => mr.Illness)
            .Include(mr => mr.Patient)
            .Include(mr => mr.Treatment)
            .Include(mr => mr.AppointmentDetails)
            .FirstOrDefault(mr => mr.MedicalRecordId == id);

            return medicalRecord;
        }

        public List<MedicalRecord> GetMedicalRecordByPatientId(int id)
        {
            return DbSet
            .Include(mr => mr.Dentist)
            .Include(mr => mr.Illness)
            .Include(mr => mr.Patient)
            .Include(mr => mr.Treatment)
            .Include(mr => mr.AppointmentDetails)
            .Where(mr => mr.MedicalRecordId == id)
            .ToList();
        }
    }
}
