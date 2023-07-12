using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Infrastructure.Repositories
{
    public class AppointmentDetailRepository : RepositoryBase<AppointmentDetail>
    {
        private readonly DentistBookingContext _context;
        public AppointmentDetailRepository(DentistBookingContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AppointmentDetail?> GetAppointmentDetailByIdAsync(int id)
        {
            try
            {
                return await DbSet
                    .Include(ad => ad.Appointment)
                    .Include(ad => ad.MedicalRecord)
                    .FirstOrDefaultAsync(ad => ad.AppointmentDetailId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }



        public async Task<List<AppointmentDetail>> GetAppointmentDetailByAppointmentIdAsync(int appointmentId)
        {
            return await DbSet
                .Include(ad => ad.Appointment)
                .Include(ad => ad.MedicalRecord)
                .Where(ad => ad.AppointmentId == appointmentId)
                .ToListAsync();
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetailsByMedicalRecordIdAsync(int medicalRecordId)
        {
            return await DbSet
                .Include(ad => ad.Appointment)
                .Include(ad => ad.MedicalRecord)
                .Where(ad => ad.MedicalRecordId == medicalRecordId)
                .ToListAsync();
        }

        public void AddMedicalRecordToAppointment(int medicalRecordId, int appointmentId)
        {
            try
            {
                var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
                var medicalRecord = _context.MedicalRecords.FirstOrDefault(mr => mr.MedicalRecordId == medicalRecordId);
                if (appointment != null && medicalRecord != null)
                {
                    // Create a new appointment detail object
                    var appointmentDetail = new AppointmentDetail
                    {
                        AppointmentId = appointmentId,
                        MedicalRecordId = medicalRecordId
                    };

                    // Add the appointment detail to the database
                    DbSet.Add(appointmentDetail);
                    _context.SaveChanges();

                    // Compute the total duration for the appointment
                    double? totalDuration = appointment.Duration;
                    var treatment = _context.Treatments.FirstOrDefault(t => t.TreatmentId == medicalRecord.TreatmentId);
                    if (treatment != null)
                    {
                        totalDuration += treatment.EstimatedTime;
                    }

                    // Update the duration of the appointment
                    appointment.Duration = totalDuration;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding medical record to appointment: {ex.Message}");
            }
        }

    }
}
