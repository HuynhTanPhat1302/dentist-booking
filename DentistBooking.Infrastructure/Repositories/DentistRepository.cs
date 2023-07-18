using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Infrastructure.Repositories
{
    public class DentistRepository : RepositoryBase<Dentist>
    {
        public DentistRepository(DentistBookingContext context) : base(context)
        {
        }

        public Dentist? GetDentistByEmail(string email)
        {
            return DbSet.FirstOrDefault(s => s.Email == email);
        }

        public async Task<bool> IsIdExisted(int id)
        {
            // Check if there is any patient with the provided email in the database
            bool isUnique = !await DbSet.AnyAsync(p => p.DentistId == id);
            return isUnique;
        }
    }
}
