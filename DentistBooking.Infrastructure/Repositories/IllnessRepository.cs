using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Infrastructure.Repositories
{
    public class IllnessRepository : RepositoryBase<Illness>
    {
        public IllnessRepository(DentistBookingContext context) : base(context)
        {
        }



        public async Task<List<Illness>> GetIllnessesAsync()
        {
            try
            {
                List<Illness> illnesses = await DbSet.ToListAsync();
                return illnesses;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the database operation
                throw new Exception(ex.ToString());
            }
        }

        public async Task<List<Illness>> SearchIllnesssAsync(string searchQuery)
        {
            try
            {
                IQueryable<Illness> query = DbSet;

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(p => p.IllnessName != null && p.IllnessName.Contains(searchQuery));
                }

                var illnesses = await query.OrderBy(p => p.IllnessName).ToListAsync();

                return illnesses;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the database operation
                throw new Exception(ex.ToString());

            }
        }


        public async Task<bool> IsIllnessNameDuplicateAsync(string illnessName)
        {
            try
            {
                // Convert the illness name to lowercase for case-insensitive comparison
                string lowercasedIllnessName = illnessName.ToLower();

                // Check if any illnesses already exist with the same name
                bool isDuplicate = await DbSet
                    .AnyAsync(i => i.IllnessName != null && i.IllnessName.ToLower() == lowercasedIllnessName);

                return isDuplicate;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the database operation
                throw new Exception(ex.ToString());
            }
        }
    }
}
