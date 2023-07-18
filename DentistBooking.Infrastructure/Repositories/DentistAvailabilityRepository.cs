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

        public async Task<List<DentistAvailability>> GetByDayOfWeekAsync(DateTime dateRequest)
        {
            var dayOfweek = dateRequest.DayOfWeek.ToString();
            var availabilities = await DbSet.Include(d => d.Dentist).Where(D => D.DayOfWeek.Equals(dayOfweek)).ToListAsync();

            return availabilities;
        }

        public async Task<List<DentistAvailability>> CheckDate(DateTime dateRequest)
        {
            var dayOfweek = dateRequest.DayOfWeek.ToString();
            var availabilities = await DbSet.Include(d => d.Dentist).Where(D => D.DayOfWeek.Equals(dayOfweek) && dateRequest.TimeOfDay>= D.StartTime && dateRequest.TimeOfDay <= D.EndTime).ToListAsync();

            return availabilities;
        }

        public List<DentistAvailability> GetByDayOfWeek(DateTime dateRequest, int dentistId)
        {
            var dayOfweek = dateRequest.DayOfWeek.ToString();
            var availabilities =  DbSet.Include(d => d.Dentist).Where(D => D.DayOfWeek.Equals(dayOfweek) && D.DentistId == dentistId).ToList();

            return availabilities;
        }
    }
}
