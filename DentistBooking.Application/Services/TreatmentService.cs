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
    public class TreatmentService : ITreatmentService
    {
        private readonly TreatmentRepository _treatmentRepository;

        public TreatmentService(TreatmentRepository treatmentRepository)        
        {
            _treatmentRepository = treatmentRepository; 
        }

        public IEnumerable<Treatment> GetAllTreatments()
        {
            return _treatmentRepository.GetAll();
        }

        public Treatment GetTreatmentById(int id)
        {
            return _treatmentRepository.GetById(id);
        }

        public void CreateTreatment(Treatment Treatment)
        {
            _treatmentRepository.Add(Treatment);
            _treatmentRepository.SaveChanges();
        }

        public void UpdateTreatment(Treatment treatment)
        {
            _treatmentRepository.Update(treatment);
            _treatmentRepository.SaveChanges();
        }

        public void DeleteTreatment(int id)
        {
            var treatment = _treatmentRepository.GetById(id);
            if (treatment != null)
            {
                _treatmentRepository.Delete(treatment);
                _treatmentRepository.SaveChanges();
            }
        }

    }
}
