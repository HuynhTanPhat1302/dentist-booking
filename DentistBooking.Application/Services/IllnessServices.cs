using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DentistBooking.Application.Services
{
    public class IllnessService : IIllnessService
    {
        private readonly IllnessRepository _illnessRepository;

        public IllnessService(IllnessRepository illnessRepository)
        {
            _illnessRepository = illnessRepository;
        }

        // public IEnumerable<Illness> GetAllIllnesss()
        // {
        //     return _illnessRepository.GetAll();
        // }

        //get-illness-by-id
        public Illness GetIllnessById(int id)
        {
            try
            {
                if (id <= 0 || id > int.MaxValue)
                {
                    throw new Exception("The iD is out of bound");
                }
                return _illnessRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //get-illnesses-async (paging)
        public async Task<List<Illness>> GetIllnessesAsync(int pageSize, int pageNumber)
        {
            try
            {
                var illnesses = await _illnessRepository.GetIllnessesAsync();

                var pagedIllnesses = illnesses
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return illnesses;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the database operation
                throw new Exception(ex.ToString());

            }
        }

        //search-illnesses (searchQuery, paging)
        public async Task<List<Illness>> SearchIllnesssAsync(int pageSize, int pageNumber, string searchQuery)
        {
            var illnesss = await _illnessRepository.SearchIllnesssAsync(searchQuery);

            var pagedIllnesss = illnesss
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedIllnesss;
        }

        //create-illness
        public void CreateIllness(Illness illness)
        {
            try
            {
                _illnessRepository.Add(illness);
                _illnessRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //update-illness
        public void UpdateIllness(Illness illness)
        {
            try
            {
                _illnessRepository.Update(illness);
                _illnessRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //delete-illness
        public void DeleteIllness(int id)
        {
            try
            {
                var illness = _illnessRepository.GetById(id);
                if (illness != null)
                {
                    _illnessRepository.Delete(illness);
                    _illnessRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
