using Microsoft.EntityFrameworkCore;


namespace DentistBooking.Infrastructure.Repositories
{
    public class TreatmentRepository : RepositoryBase<Treatment>
    {
        public TreatmentRepository(DentistBookingContext context) : base(context)
        {
        }

        public async Task<List<Treatment>> GetTreatmentesAsync()
        {
            try
            {
                List<Treatment> treatmentes = await DbSet.Include(i => i.MedicalRecords).ToListAsync();
                return treatmentes;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the database operation
                throw new Exception(ex.ToString());
            }
        }

        public async Task<List<Treatment>> SearchTreatmentsAsync(string searchQuery)
        {
            try
            {
                IQueryable<Treatment> query = DbSet.Include(i => i.MedicalRecords);

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(p => p.TreatmentName != null && p.TreatmentName.Contains(searchQuery));
                }

                var treatmentes = await query.OrderBy(p => p.TreatmentName).ToListAsync();

                return treatmentes;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the database operation
                throw new Exception(ex.ToString());

            }
        }

        public async Task<bool> IsTreatmentNameDuplicateAsync(string treatmentName)
        {
            try
            {
                // Convert the treatment name to lowercase for case-insensitive comparison
                string lowercasedTreatmentName = treatmentName.ToLower();

                // Check if any treatmentes already exist with the same name
                bool isDuplicate = await DbSet
                    .AnyAsync(i => i.TreatmentName != null && i.TreatmentName.ToLower() == lowercasedTreatmentName);

                return isDuplicate;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the database operation
                throw new Exception(ex.ToString());
            }
        }


        // Add any additional custom methods or queries specific to the Treatment entity here
    }
}
