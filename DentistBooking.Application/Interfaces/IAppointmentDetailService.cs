using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IAppointmentDetailService
    {
        AppointmentDetail GetAppointmentDetailById(int id);

        Task<AppointmentDetail?> GetAppointmentDetailByIdAsync(int id);

        Task<IEnumerable<AppointmentDetail>> GetAppointmentDetailsByMedicalRecordIdAsync(int medicalRecordId);

        Task<IEnumerable<AppointmentDetail>> GetAppointmentDetailByAppointmentIdAsync(int appointmentId);

        void CreateAppointmentDetail(AppointmentDetail appointmentDetail);

        void UpdateAppointmentDetail(AppointmentDetail appointmentDetail);

        void DeleteAppointmentDetail(int id);
    }
}
