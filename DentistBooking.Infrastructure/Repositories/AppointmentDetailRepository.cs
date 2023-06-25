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
        public AppointmentDetailRepository(DentistBookingContext context) : base(context)
        {
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


    }
}
