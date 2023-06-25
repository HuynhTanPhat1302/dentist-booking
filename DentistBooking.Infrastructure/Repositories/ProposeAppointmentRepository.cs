using Microsoft.EntityFrameworkCore;


namespace DentistBooking.Infrastructure.Repositories
{
    public class ProposeAppointmentRepository : RepositoryBase<ProposeAppointment>
    {
        public ProposeAppointmentRepository(DentistBookingContext context) : base(context)
        {
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





        // Add any additional custom methods or queries specific to the ProposeAppointment entity here
    }
}
