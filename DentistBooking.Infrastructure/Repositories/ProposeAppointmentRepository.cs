using Microsoft.EntityFrameworkCore;


namespace DentistBooking.Infrastructure.Repositories
{
    public class ProposeAppointmentRepository : RepositoryBase<ProposeAppointment>
    {

        private readonly DentistBookingContext _context;

        public ProposeAppointmentRepository(DentistBookingContext context) : base(context)
        {
            _context = context;
        }


        public async Task<ProposeAppointment?> GetProposeAppointmentByNameAsync(string name)
        {
            var proposeAppointment = await DbSet.FirstOrDefaultAsync(s => s.Name == name);
            return proposeAppointment;
        }

        public async Task<List<ProposeAppointment>> SearchProposeAppointmentsAsync(string searchQuery)
        {
            var proposeAppointments = await DbSet.Where(s => (s.Name != null && s.Name.Contains(searchQuery)) || (s.PhoneNumber != null && s.PhoneNumber.Contains(searchQuery)))
            .OrderBy(s => s.Name)
            .ToListAsync();
            return proposeAppointments;
        }

        public async Task<List<ProposeAppointment>> GetProposeAppointmentsByStatusAsync(string status)
        {
            try
            {
                if (!Enum.TryParse(status, true, out ProposeAppointmentStatus statusEnum))
                {
                    throw new ArgumentException("Invalid status value.", nameof(status));
                }

                var proposeAppointments = await DbSet
                    .Where(s => s.Status == statusEnum)
                    .OrderBy(s => s.Datetime)
                    .ToListAsync();

                return proposeAppointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<List<ProposeAppointment>> GetProposeAppointmentsByEmailAsync(string email)
        {
            try
            {
                var proposeAppointments = await _context.ProposeAppointments
                    .Include(pa => pa.Patient)
                    .Where(pa => pa.Patient != null && pa.Patient.Email == email)
                    .ToListAsync();

                return proposeAppointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public ProposeAppointment? GetByIdV2(int id)
        {
            try
            {
                var proposeAppointment = DbSet.Find(id);
                return proposeAppointment;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving ProposeAppointment with ID {id}: {ex.Message}");
            }
        }





        // Add any additional custom methods or queries specific to the ProposeAppointment entity here
    }
}
