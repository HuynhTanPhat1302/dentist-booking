using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IIllnessService
    {
        Illness GetIllnessById(int id);

        Task<List<Illness>> GetIllnessesAsync(int pageSize, int pageNumber);

        Task<List<Illness>> SearchIllnesssAsync(int pageSize, int pageNumber, string searchQuery);

        void CreateIllness(Illness illness);

        void UpdateIllness(Illness illness);

         void DeleteIllness(int id);
    }
}
