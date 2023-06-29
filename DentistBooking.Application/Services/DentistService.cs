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

        public Dentist? GetDentistById(int id)
        {
            return _dentistRepository.GetById(id);
        }

        public Dentist CreateDentist(Dentist dentist)
        {
            var email = _dentistRepository.GetDentistByEmail(dentist.Email);
            if (email != null)
            {
                throw new Exception("Dentist Email is existed!");
            }
            _dentistRepository.Add(dentist);
            _dentistRepository.SaveChanges();
            return dentist;
        }

        public Dentist UpdateDentist(int id, Dentist dentist)
        {
            var existedDentist = _dentistRepository.GetById(id);
            if(existedDentist == null) 
            {
                throw new Exception("Dentist is not existed");
            }
            var emailIsExisted = _dentistRepository.GetDentistByEmail(dentist.Email) != null;
            if (emailIsExisted)
            {
                throw new Exception("Email is existed");
            }
            existedDentist.DentistName = dentist.DentistName;
            existedDentist.Email = dentist.Email;
            existedDentist.PhoneNumber= dentist.PhoneNumber;
            _dentistRepository.Update(existedDentist);
            _dentistRepository.SaveChanges();
            return existedDentist;
        }

        public void DeleteDentist(int id)
        {
            var dentist = _dentistRepository.GetById(id);
            if (dentist != null)
            {
                _dentistRepository.Delete(dentist);
                _dentistRepository.SaveChanges();
            }
            else
            {
                throw new Exception("Dentist is not existed");
            }
        }

        public Dentist? GetDentistByEmail(string email)
        {
            return _dentistRepository.GetDentistByEmail(email);
        }

        public async Task<List<Dentist>> GetDentistsAsync(int pageSize, int pageNumber)
        {
            var dentists = await _dentistRepository.GetDentistAsync();

            var pagedDentists = dentists
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedDentists;
        }

        public async Task<List<Dentist>> SearchDentistsAsync(int pageSize, int pageNumber, string searchQuery)
        {
            var dentists = await _dentistRepository.SearchDentistsAsync(searchQuery);

            var pagedDentists = dentists
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedDentists;
        }
    }
}
