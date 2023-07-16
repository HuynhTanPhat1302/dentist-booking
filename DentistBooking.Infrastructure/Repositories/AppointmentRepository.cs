
using Microsoft.EntityFrameworkCore;

namespace DentistBooking.Infrastructure.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>
    {
        public AppointmentRepository(DentistBookingContext context) : base(context)
        {
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            try
            {
                List<Appointment> appointments = await DbSet.Include(a => a.Staff)
                .Include(a => a.Patient)
                .Include(a => a.Staff)
                .Include(a => a.AppointmentDetails)
                .ToListAsync();
                return appointments;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the database operation
                throw new Exception(ex.ToString());
            }
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            try
            {
                var appointment = await DbSet
                    .Include(a => a.Staff)
                    .Include(a => a.Patient)
                    .Include(a => a.AppointmentDetails)
                    .FirstOrDefaultAsync(a => a.AppointmentId == id);

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<List<Appointment>> GetAppointmentsByDentistIdAsync(int dentistId)
        {
            try
            {
                List<Appointment> appointments = await DbSet
                    .Include(a => a.Patient)
                    .Include(a => a.Staff)
                    .Include(a => a.AppointmentDetails)
                    .Where(a => a.StaffId == dentistId)
                    .ToListAsync();

                return appointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        public async Task<List<Appointment>> GetAppointmentsByDentistIdAndDateTimeAsync(int dentistId, DateTime date)
        {
            try
            {
                List<Appointment> appointments = await DbSet
                    .Include(a => a.Patient)
                    .Include(a => a.Staff)
                    .Include(a => a.AppointmentDetails)
                    .Where(a => a.DentistId == dentistId && a.Datetime.Value.Date == date.Date).OrderBy(a => a.Datetime)
                    .ToListAsync();

                return appointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        public async Task<List<Appointment>> CheckAppointmentIsExistedInWorking(int dentistId, TimeSpan startDate, TimeSpan endDate)
        {
            try
            {
                List<Appointment> appointments = await DbSet
                    .Include(a => a.Patient)
                    .Include(a => a.Staff)
                    .Include(a => a.AppointmentDetails).OrderBy(a => a.Datetime)
                    .Where(a => a.DentistId == dentistId && (a.Datetime.Value.TimeOfDay >= startDate && a.Datetime.Value.AddHours((double)a.Duration).TimeOfDay <= endDate))
                    .ToListAsync();

                return appointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        public List<Appointment> GetAppointmentsByDentistIdAndDateTime(int dentistId, DateTime date)
        {
            try
            {
                List<Appointment> appointments = DbSet
                    .Where(a => a.DentistId == dentistId && a.Datetime.Equals(date)).ToList();

                return appointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            try
            {
                List<Appointment> appointments = await DbSet
                    .Include(a => a.Patient)
                    .Include(a => a.Staff)
                    .Include(a => a.AppointmentDetails)
                    .Where(a => a.PatientId == patientId)
                    .ToListAsync();

                return appointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }



        public IEnumerable<Appointment> GetAppointmentsByDentistId(int dentistId)
        {
            return DbSet.Include(a => a.Dentist)
                .Include(a => a.Patient)
                .Include(a => a.Staff)
                .Where(a => a.DentistId == dentistId)
                .ToList();
        }

        public Appointment? GetAppointmentsById(int id)
        {
            return DbSet.Include(a => a.Dentist)
                .Include(a => a.Patient)
                .Include(a => a.Staff)
                .FirstOrDefault(a => a.AppointmentId == id);
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

        public Appointment? GetAppointmentsByStaffId(int appointmentId)
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
