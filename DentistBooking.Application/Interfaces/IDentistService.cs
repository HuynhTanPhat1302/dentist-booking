using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IDentistService
    {
        IEnumerable<Dentist> GetAllDentists();
        Dentist GetDentistById(int id);
        Dentist CreateDentist(Dentist dentist);
        Dentist UpdateDentist(int id, Dentist dentist);
        void DeleteDentist(int id);
        Dentist? GetDentistByEmail(string email);


    }
}
