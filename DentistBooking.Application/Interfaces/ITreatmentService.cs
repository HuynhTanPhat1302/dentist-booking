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
        IEnumerable<Treatment> GetAllTreatments();
        Treatment GetTreatmentById(int id);
        void CreateTreatment(Treatment Treatment);
        void UpdateTreatment(Treatment Treatment);
        void DeleteTreatment(int id);


    }
}
