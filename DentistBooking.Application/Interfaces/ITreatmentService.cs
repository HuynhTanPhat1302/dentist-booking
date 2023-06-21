using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface ITreatmentService
    {
        
        Treatment GetTreatmentById(int id);

        Task<List<Treatment>> GetTreatmentesAsync(int pageSize, int pageNumber);


        Task<List<Treatment>> SearchTreatmentsAsync(int pageSize, int pageNumber, string searchQuery);

        void CreateTreatment(Treatment Treatment);
        void UpdateTreatment(Treatment Treatment);
        void DeleteTreatment(int id);


    }
}
