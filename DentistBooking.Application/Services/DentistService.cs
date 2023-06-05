using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Services
{
    public class DentistService : IDentistService
    {
        private readonly DentistRepository _dentistRepository;

        public DentistService(DentistRepository dentistRepository)
        {
            _dentistRepository = dentistRepository;
        }

        public IEnumerable<Dentist> GetAllDentists()
        {
            return _dentistRepository.GetAll();
        }

        public Dentist GetDentistById(int id)
        {
            return _dentistRepository.GetById(id);
        }

        public void CreateDentist(Dentist dentist)
        {
            _dentistRepository.Add(dentist);
            _dentistRepository.SaveChanges();
        }

        public void UpdateDentist(Dentist dentist)
        {
            _dentistRepository.Update(dentist);
            _dentistRepository.SaveChanges();
        }

        public void DeleteDentist(int id)
        {
            var dentist = _dentistRepository.GetById(id);
            if (dentist != null)
            {
                _dentistRepository.Delete(dentist);
                _dentistRepository.SaveChanges();
            }
        }

        public Dentist? GetDentistByEmail(string email)
        {
            return _dentistRepository.GetDentistByEmail(email);

        }
    }
}
