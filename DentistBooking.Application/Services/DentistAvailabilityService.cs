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
        private readonly AppointmentRepository _appointmentRepository;
        public DentistAvailabilityService(DentistRepository dentistRepository, DentistAvailabilityRepository dentistAvailabilityRepository, AppointmentRepository appointmentRepository)
        {
            _dentistAvailabilityRepository = dentistAvailabilityRepository;
            _dentistRepository = dentistRepository;
            _appointmentRepository = appointmentRepository;
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

        public async Task<Dictionary<string, (int Start, int End)>> GetFreeTime(DateTime date)
        {
            Dictionary<string, (int Start, int End)> dentistAvailability = new Dictionary<string, (int Start, int End)>();
            var dentistWorkings = await _dentistAvailabilityRepository.GetDentistAvailabilitiesByDayOfWeek(date);
            TimeSpan startTime;
            TimeSpan endTime;
            foreach(var dentistBusy in dentistWorkings)
            {
                
                var dentistAppointments = await _appointmentRepository.GetAppointmentsByDentistIdAsync((int)dentistBusy.DentistId);
                for (int i = 0; i < dentistAppointments.Count; i++)
                {
                    if (dentistAppointments[i].DentistId == dentistBusy.DentistId)
                    {
                        startTime = TimeSpan.FromHours(dentistBusy.StartTime.Value.Hours);
                        endTime = TimeSpan.FromHours(dentistBusy.EndTime.Value.Hours);
                        var endTimeAppointment = dentistAppointments[i].Datetime.Value.AddHours((double)dentistAppointments[i].Duration);
                        if (dentistAppointments[i].Datetime.Value.TimeOfDay > dentistBusy.StartTime && endTimeAppointment.TimeOfDay < dentistBusy.EndTime)
                        {
                            endTime = dentistAppointments[i].Datetime.Value.TimeOfDay;
                            dentistAvailability.Add(dentistBusy.Dentist.Email, (startTime.Hours, endTime.Hours));
                            /*if (dentistAppointments[i + 1] != null)
                            {
                                startTime = TimeSpan.FromHours(endTimeAppointment.Hour);
                                endTime = dentistAppointments[i++].Datetime.Value.TimeOfDay;
                                dentistAvailability.Add(dentistBusy.Dentist.Email, (startTime, endTime));
                            }*/
                        }else
                        {
                            startTime = TimeSpan.FromHours(dentistBusy.StartTime.Value.Hours);
                            endTime = TimeSpan.FromHours(dentistBusy.EndTime.Value.Hours);
                            dentistAvailability.Add(dentistBusy.Dentist.Email, (startTime.Hours, endTime.Hours));
                        }
                    }
                }
            }
            return dentistAvailability;
        }
    }
}
