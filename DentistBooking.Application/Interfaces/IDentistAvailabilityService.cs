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
        Task<List<DentistAvailability>> GetDentistAvailabilitys(int pageSize, int pageNumber);

        Task<List<DentistAvailability>> SearchDentistAvailabilitysAsync(int pageSize, int pageNumber, string searchQuery);

        DentistAvailability CreateDentistAvailabilitys(DentistAvailability dentistAvailability);

        DentistAvailability UpdateDentistAvailabilitys(int id, DentistAvailability dentistAvailability);

        DentistAvailability GetById(int id);
        List<DentistAvailability> GetDentistAvailabilitysByDayOfWeek(DateTime date, int dentistId);
        Task<bool> CheckEstimatedTimeIsExistedInFreeTiemAvailability(DateTime date);
        //Task<Dictionary<string, (TimeSpan Start, TimeSpan End)>> GetDentistFreetimeAvailability(DateTime dateRequest);
        Task<Dictionary<string, List<(TimeSpan Start, TimeSpan End)>>> GetDentistFreeTimeAvailability(DateTime dateRequest);
        void DeleteDentistAvailability(int id);
    }
}
