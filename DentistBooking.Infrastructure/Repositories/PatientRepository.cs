using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Infrastructure.Repositories
{
    public class PatientRepository : RepositoryBase<Patient>
    {
        public PatientRepository(DentistBookingContext context) : base(context)
        {
        }

        public Patient? GetByEmail(string email)
        {
            return DbSet.Include(p => p.Appointments)
                .Include(p => p.MedicalRecords)
                .SingleOrDefault(p => p.Email == email);    
        }

        // Add any additional custom methods or queries specific to the Patient entity here
    }
}
