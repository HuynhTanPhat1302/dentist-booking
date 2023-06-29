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

        public async Task<List<Dentist>> SearchDentistsAsync(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                IQueryable<Dentist> query = DbSet.Where(p => p.DentistName != null && p.DentistName.Contains(searchQuery));
            }

            var dentists = await DbSet.OrderBy(p => p.DentistName).ToListAsync();

            return dentists;
        }

        public async Task<List<Dentist>> GetDentistAsync()
        {
            var dentist = await DbSet
                .OrderBy(p => p.DentistName)
                .ToListAsync();

            return dentist;
        }
    }
}
