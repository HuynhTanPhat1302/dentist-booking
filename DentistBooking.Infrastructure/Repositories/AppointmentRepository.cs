
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

        public List<Appointment> GetAppointmentsByDentistEmail(string email)
        {
            return DbSet
                .Include(a => a.Dentist)
                .Include(a => a.Patient)
                .Include(a => a.Staff)
                .Where(a => a.Dentist != null && a.Dentist.Email == email)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByPatientEmail(string email)
        {
            return DbSet
                .Include(a => a.Dentist)
                .Include(a => a.Patient)
                .Include(a => a.Staff)
                .Where(a => a.Patient != null && a.Patient.Email == email)
                .ToList();
        }

        public Appointment GetAppointmentsByStaffId(int appointmentId)
        {
            return DbSet.Include(a => a.Dentist)
                .Include(a => a.Patient)
                .Include(a => a.Staff)
                .Where(a => a.AppointmentId == appointmentId).FirstOrDefault();
        }

        public List<Appointment> GetAllAppointments()
        {
            return DbSet.Include(a => a.Dentist)
                .Include(a => a.Patient)
                .Include(a => a.Staff)
                .Include(a => a.AppointmentDetails)
                .ToList();
        }



        // Add any additional custom methods or queries specific to the Appointment entity here
    }
}
