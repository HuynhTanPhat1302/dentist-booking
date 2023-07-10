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
    public class TreatmentService : ITreatmentService
    {
        private readonly TreatmentRepository _treatmentRepository;

        public TreatmentService(TreatmentRepository treatmentRepository)
        {
            _treatmentRepository = treatmentRepository;
        }

        //get-treatmentes-async (paging)
        public async Task<List<Treatment>> GetTreatmentesAsync(int pageSize, int pageNumber)
        {
            try
            {
                var treatmentes = await _treatmentRepository.GetTreatmentesAsync();

                var pagedTreatmentes = treatmentes
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return treatmentes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        public Treatment GetTreatmentById(int id)
        {
            try
            {
                if (id <= 0 || id > int.MaxValue)
                {
                    throw new Exception("The iD is out of bound");
                }
                return _treatmentRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //search-treatmentes (searchQuery, paging)
        public async Task<List<Treatment>> SearchTreatmentsAsync(int pageSize, int pageNumber, string searchQuery)
        {
            var treatments = await _treatmentRepository.SearchTreatmentsAsync(searchQuery);

            var pagedTreatments = treatments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedTreatments;
        }

        public void CreateTreatment(Treatment treatment)
        {
            try
            {
                _treatmentRepository.Add(treatment);
                _treatmentRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //update-treatment
        public void UpdateTreatment(Treatment treatment)
        {
            try
            {
                _treatmentRepository.Update(treatment);
                _treatmentRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //delete-treatment
        public void DeleteTreatment(int id)
        {
            try
            {
                var treatment = _treatmentRepository.GetById(id);
                if (treatment != null)
                {
                    _treatmentRepository.Delete(treatment);
                    _treatmentRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public Treatment GetTreatmentByEstimatedTime(double treatmentTime)
        {
            try
            {
                var treatment = _treatmentRepository.GetAll().FirstOrDefault(t => t.EstimatedTime == treatmentTime);
                if (treatment == null)
                {
                    throw new Exception("Treatment's estimated time is not existed!");
                }
                return treatment;
            }catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
