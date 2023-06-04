using Microsoft.EntityFrameworkCore;


namespace DentistBooking.Infrastructure.Repositories
{
    public class TreatmentRepository : RepositoryBase<Treatment>
    {
        public TreatmentRepository(DentistBookingContext context) : base(context)
        {
        }


        // Add any additional custom methods or queries specific to the Treatment entity here
    }
}
