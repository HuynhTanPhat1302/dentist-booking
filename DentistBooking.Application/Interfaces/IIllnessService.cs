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
        IEnumerable<Illness> GetAllIllnesss();
        Illness GetIllnessById(int id);
        void CreateIllness(Illness illness);
        void UpdateIllness(Illness illness);
        void DeleteIllness(int id);
    }
}
