using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentistBooking.Application.Interfaces;

namespace DentistBooking.Application.Services
{
    public class DentistAvailabilityService : IDentistAvailabilityService
    {
        private readonly DentistAvailabilityRepository _dentistAvaiRepository;

        public DentistAvailabilityService(DentistAvailabilityRepository dentistAvaiRepository)
        {
            _dentistAvaiRepository = dentistAvaiRepository;
        }

        public IEnumerable<DentistAvailability> GetAllDentistbvailabilities()
        {
            return _dentistAvaiRepository.GetAll();
        }

        public DentistAvailability GetDentistAvailabilityById(int id)
        {
            return _dentistAvaiRepository.GetById(id);
        }

        public void CreateDentistAvailability(DentistAvailability dentistAvai)
        {
            _dentistAvaiRepository.Add(dentistAvai);
            _dentistAvaiRepository.SaveChanges();
        }

        public void UpdateDentistAvailability(DentistAvailability dentistAvai)
        {
            _dentistAvaiRepository.Update(dentistAvai);
            _dentistAvaiRepository.SaveChanges();
        }

        public void DeleteDentistAvailability(int id)
        {
            var dentistAvai = _dentistAvaiRepository.GetById(id);
            if (dentistAvai != null)
            {
                _dentistAvaiRepository.Delete(dentistAvai);
                _dentistAvaiRepository.SaveChanges();
            }
        }

    }
}
