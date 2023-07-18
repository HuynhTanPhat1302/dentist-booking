
using Microsoft.EntityFrameworkCore;

namespace DentistBooking.Infrastructure.Repositories
{
    public class StaffRepository : RepositoryBase<staff>
    {
        public StaffRepository(DentistBookingContext context) : base(context)
        {
        }



        // Add any additional custom methods or queries specific to the Staff entity here
        public staff? GetStaffByEmail(string email)
        {
            return DbSet.FirstOrDefault(s => s.Email == email);
        }

       public async Task<bool> IsEmailUnique(string email)
        {
            // Check if there is any patient with the provided email in the database
            bool isUnique = !await DbSet.AnyAsync(p => p.Email == email);
            return isUnique;
        }


    }
}
