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
    }
}
