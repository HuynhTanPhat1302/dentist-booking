using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Infrastructure.Repositories
{
    public class DentistAvailabilityRepository : RepositoryBase<DentistAvailability>
    {
        public DentistAvailabilityRepository(DentistBookingContext context) : base(context)
        {

        }
        public async Task<List<DentistAvailability>> SearchdentistAvailabilitiesAsync(string searchQuery)
        {
            IQueryable<DentistAvailability> query = DbSet.Include(p => p.Dentist);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.Dentist.DentistName != null && p.Dentist.DentistName.Contains(searchQuery));
            }

            var dentistAvailabilities = await query.OrderBy(p => p.Dentist.DentistName).ToListAsync();

            return dentistAvailabilities;
        }

        public DentistAvailability? GetById(int id)
        {
            return DbSet.Include(d => d.Dentist).SingleOrDefault(d => d.AvailabilityId == id);
        }

        public async Task<List<DentistAvailability>> GetdentistAvailabilitiesAsync()
        {
            var dentistAvailabilities = await DbSet
                .Include(p => p.Dentist)
                .OrderBy(p => p.Dentist.DentistName)
                .ToListAsync();

            return dentistAvailabilities;
        }

        public async Task<List<DentistAvailability>> GetAllDentistAvailability()
        {
            var res = await DbSet.Include(d => d.Dentist).ToListAsync();
            return res;
        }

        public async Task<List<DentistAvailability>> GetDentistAvailabilitiesByDayOfWeek(DateTime date)
        {
            var dayOfWeekString = date.DayOfWeek.ToString();
            var availabilities = await DbSet.Include(d => d.Dentist).ToListAsync();

            return availabilities.Where(d => d.DayOfWeek == dayOfWeekString).ToList();
        }

    }
}
