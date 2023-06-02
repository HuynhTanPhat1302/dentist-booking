using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IDentistAvailabilityService
    {
        IEnumerable<DentistAvailability> GetAllDentistbvailabilities();
        DentistAvailability GetDentistAvailabilityById(int id);
        void CreateDentistAvailability(DentistAvailability dentistAvai);
        void UpdateDentistAvailability(DentistAvailability dentistAvai);
        void DeleteDentistAvailability(int id);
    }
}
