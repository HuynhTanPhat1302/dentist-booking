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
    public class IllnessService : IIllnessService
    {
        private readonly IllnessRepository _illnessRepository;
        public IllnessService(IllnessRepository illnessRepository)
        {
            _illnessRepository = illnessRepository;
        }
        public IEnumerable<Illness> GetAllIllnesss()
        {
            return _illnessRepository.GetAll();
        }

        public Illness GetIllnessById(int id)
        {
            return _illnessRepository.GetById(id);
        }

        public void CreateIllness(Illness illness)
        {
            _illnessRepository.Add(illness);
            _illnessRepository.SaveChanges();
        }

        public void UpdateIllness(Illness illness)
        {
            _illnessRepository.Update(illness);
            _illnessRepository.SaveChanges();
        }

        public void DeleteIllness(int id)
        {
            var illness = _illnessRepository.GetById(id);
            if (illness != null)
            {
                _illnessRepository.Delete(illness);
                _illnessRepository.SaveChanges();
            }
        }

        
    }
}
