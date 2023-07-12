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
    public class AppointmentDetailService : IAppointmentDetailService
    {
        private readonly AppointmentDetailRepository _appointmentDetailRepository;

        public AppointmentDetailService(AppointmentDetailRepository appointmentDetailRepository)
        {
            _appointmentDetailRepository = appointmentDetailRepository;
        }

        //get-appointmentDetail-by-id
        public AppointmentDetail GetAppointmentDetailById(int id)
        {
            try
            {

                return _appointmentDetailRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<AppointmentDetail?> GetAppointmentDetailByIdAsync(int id)
        {
            try
            {


                return await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //get appointment Detail by medical record id
        public async Task<IEnumerable<AppointmentDetail>> GetAppointmentDetailsByMedicalRecordIdAsync(int medicalRecordId)
        {
            try
            {


                return await _appointmentDetailRepository.GetAppointmentDetailsByMedicalRecordIdAsync(medicalRecordId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<IEnumerable<AppointmentDetail>> GetAppointmentDetailByAppointmentIdAsync(int appointmentId)
        {
            try
            {


                return await _appointmentDetailRepository.GetAppointmentDetailByAppointmentIdAsync(appointmentId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void AddMedicalRecordToAppointment(AppointmentDetail appointmentDetail)
        {
            try
            {
                _appointmentDetailRepository.AddMedicalRecordToAppointment(appointmentDetail.MedicalRecordId ?? 0, appointmentDetail.AppointmentId ?? 0);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding medical record to appointment detail: {ex.Message}");
            }
        }





        //create-appointmentDetail
        public void CreateAppointmentDetail(AppointmentDetail appointmentDetail)
        {
            try
            {
                _appointmentDetailRepository.Add(appointmentDetail);
                _appointmentDetailRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //update-appointmentDetail
        public void UpdateAppointmentDetail(AppointmentDetail appointmentDetail)
        {
            try
            {
                _appointmentDetailRepository.Update(appointmentDetail);
                _appointmentDetailRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //delete-appointmentDetail
        public void DeleteAppointmentDetail(int id)
        {
            try
            {
                var appointmentDetail = _appointmentDetailRepository.GetById(id);
                if (appointmentDetail != null)
                {
                    _appointmentDetailRepository.Delete(appointmentDetail);
                    _appointmentDetailRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


    }
}
