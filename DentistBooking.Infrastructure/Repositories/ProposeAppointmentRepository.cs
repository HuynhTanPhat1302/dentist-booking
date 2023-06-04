using Microsoft.EntityFrameworkCore;


namespace DentistBooking.Infrastructure.Repositories
{
    public class ProposeAppointmentRepository : RepositoryBase<ProposeAppointment>
    {
        public ProposeAppointmentRepository(DentistBookingContext context) : base(context)
        {
        }

        
        // Add any additional custom methods or queries specific to the ProposeAppointment entity here
    }
}
