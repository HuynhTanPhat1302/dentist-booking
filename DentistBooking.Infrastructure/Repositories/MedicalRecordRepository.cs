using Microsoft.EntityFrameworkCore;


namespace DentistBooking.Infrastructure.Repositories
{
    public class MedicalRecordRepository : RepositoryBase<MedicalRecord>
    {
        public MedicalRecordRepository(DentistBookingContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsAsync()
        {
            try
            {
                return await DbSet
                    .Include(mr => mr.Dentist)
                    .Include(mr => mr.Illness)
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Treatment)
                    .Include(mr => mr.AppointmentDetails)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public List<MedicalRecord> GetMedicalRecordByPatientEmail(string email)
        {
            var medicalRecord = DbSet
                .Include(mr => mr.Dentist)
                .Include(mr => mr.Illness)
                .Include(mr => mr.Patient)
                .Include(mr => mr.Treatment)
                .Include(mr => mr.AppointmentDetails)
                .Where(mr => mr.Patient != null && mr.Patient.Email == email)
                .ToList();
            return medicalRecord;
        }




        public MedicalRecord? GetMedicalRecordById(int id)
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
