
using Microsoft.EntityFrameworkCore;

namespace DentistBooking.Infrastructure.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>
    {
        public AppointmentRepository(DentistBookingContext context) : base(context)
        {
        }

        

        public IEnumerable<Appointment> GetAppointmentsByDentistId(int dentistId)
        {
            return DbSet.Include(a => a.Dentist)
                .Include(a => a.Patient)
                .Include(a => a.Staff)
                .Where(a => a.DentistId == dentistId)
                .ToList();
        }

        public IEnumerable<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return DbSet.Include(a => a.Dentist)
                .Include(a => a.Patient)
                .Include(a => a.Staff)
                .Where(a => a.PatientId == patientId)
                .ToList();
        }

        // Add any additional custom methods or queries specific to the Appointment entity here
    }
}
