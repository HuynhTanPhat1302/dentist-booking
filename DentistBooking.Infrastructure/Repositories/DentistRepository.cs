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
    }
}
