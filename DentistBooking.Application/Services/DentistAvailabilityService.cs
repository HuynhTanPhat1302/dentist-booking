using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure;
using DentistBooking.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Services
{
    public class DentistAvailabilityService : IDentistAvailabilityService
    {
        private readonly DentistRepository _dentistRepository;
        private readonly DentistAvailabilityRepository _dentistAvailabilityRepository;
        public DentistAvailabilityService(DentistRepository dentistRepository, DentistAvailabilityRepository dentistAvailabilityRepository)
        {
            _dentistAvailabilityRepository = dentistAvailabilityRepository;
            _dentistRepository = dentistRepository;
        }
        public DentistAvailability CreateDentistAvailabilitys(DentistAvailability dentistAvailability)
        {
            var dentistIsExisted = _dentistRepository.GetById((int)dentistAvailability.DentistId) == null;
            if(dentistIsExisted)
            {
                throw new Exception("Dentist is not existed");
            }
            _dentistAvailabilityRepository.Add(dentistAvailability);
            _dentistAvailabilityRepository.SaveChanges();
            return dentistAvailability;
        }

        public void DeleteDentistAvailability(int id)
        {
            var dentistAvailabilityIsExisted = _dentistAvailabilityRepository.GetById(id);
            if (dentistAvailabilityIsExisted == null)
            {
                throw new Exception("Dentist Availability is not existed!");
            }
            _dentistAvailabilityRepository.Delete(dentistAvailabilityIsExisted);
            _dentistAvailabilityRepository.SaveChanges();
        }

        public async Task<List<DentistAvailability>> GetDentistAvailabilitys(int pageSize, int pageNumber)
        {
            var dentistAvailabilities = await _dentistAvailabilityRepository.GetdentistAvailabilitiesAsync();

            var pagedDentistAvailabilities = dentistAvailabilities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedDentistAvailabilities;
        }

        public async Task<List<DentistAvailability>> SearchDentistAvailabilitysAsync(int pageSize, int pageNumber, string searchQuery)
        {
            var dentistAvailabilities = await _dentistAvailabilityRepository.SearchdentistAvailabilitiesAsync(searchQuery);

            var pagedDentistAvailabilities = dentistAvailabilities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList(); 

            return pagedDentistAvailabilities;
        }

        public DentistAvailability UpdateDentistAvailabilitys(int id, DentistAvailability dentistAvailability)
        {
            var dentistAvailabilityIsExisted = _dentistAvailabilityRepository.GetById(id);
            if(dentistAvailabilityIsExisted == null)
            {
                throw new Exception("Dentist Availability is not existed!");
            }
            var dentistIsExisted = _dentistRepository.GetById((int)dentistAvailability.DentistId);
            if (dentistIsExisted == null)
            {
                throw new Exception("Dentist is not existed");
            }

            dentistAvailabilityIsExisted.DentistId = dentistAvailability.DentistId;
            dentistAvailabilityIsExisted.EndTime = dentistAvailability.EndTime;
            dentistAvailabilityIsExisted.StartTime = dentistAvailability.StartTime;
            dentistAvailabilityIsExisted.Dentist = dentistIsExisted;
            _dentistAvailabilityRepository.Update(dentistAvailabilityIsExisted);
            _dentistAvailabilityRepository.SaveChanges();
            return dentistAvailabilityIsExisted;
        }

        public DentistAvailability GetById(int id)
        {
            return _dentistAvailabilityRepository.GetById(id);
        }
    }
}
