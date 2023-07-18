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
            if (dentistIsExisted)
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
            if (dentistAvailabilityIsExisted == null)
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

        public async Task<Dictionary<string, List<(TimeSpan Start, TimeSpan End)>>> GetDentistFreeTimeAvailability(DateTime dateRequest)
        {
            var dentistFreeTimeAvailability = new Dictionary<string, List<(TimeSpan Start, TimeSpan End)>>();

            var listDentistAvailability = await _dentistAvailabilityRepository.GetByDayOfWeekAsync(dateRequest);
            if (listDentistAvailability.Count == 0)
            {
                throw new Exception($"No dentist working on {dateRequest.DayOfWeek}");
            }

            foreach (var dentistAvailability in listDentistAvailability)
            {
                var dentistName = dentistAvailability.Dentist.DentistName;
                var startTime = dentistAvailability.StartTime.Value;
                var endTime = dentistAvailability.EndTime.Value;

                var listDentistAppointments = await _appointmentRepository.GetAppointmentsByDentistIdAndDateTimeAsync((int)dentistAvailability.DentistId, dateRequest);

                var freeTimeSlots = new List<(TimeSpan Start, TimeSpan End)>();

                if (listDentistAppointments.Count == 0)
                {
                    freeTimeSlots.Add((startTime, endTime));
                }
                else
                {
                    // Sort the appointments by start time
                    listDentistAppointments.Sort((a, b) => a.Datetime.Value.TimeOfDay.CompareTo(b.Datetime.Value.TimeOfDay));

                    // Check if there's any free time before the first appointment
                    if (startTime < listDentistAppointments[0].Datetime.Value.TimeOfDay)
                    {
                        freeTimeSlots.Add((startTime, listDentistAppointments[0].Datetime.Value.TimeOfDay));
                    }

                    // Check for free time between appointments
                    for (int i = 0; i < listDentistAppointments.Count - 1; i++)
                    {
                        var currentAppointmentEndTime = listDentistAppointments[i].Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)listDentistAppointments[i].Duration));
                        var nextAppointmentStartTime = listDentistAppointments[i + 1].Datetime.Value.TimeOfDay;

                        if (currentAppointmentEndTime < nextAppointmentStartTime)
                        {
                            freeTimeSlots.Add((currentAppointmentEndTime, nextAppointmentStartTime));
                        }
                    }

                    // Check if there's any free time after the last appointment
                    if (endTime > listDentistAppointments[listDentistAppointments.Count - 1].Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)listDentistAppointments[listDentistAppointments.Count - 1].Duration)))
                    {
                        freeTimeSlots.Add((listDentistAppointments[listDentistAppointments.Count - 1].Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)listDentistAppointments[listDentistAppointments.Count - 1].Duration)), endTime));
                    }
                }

                dentistFreeTimeAvailability.Add(dentistName, freeTimeSlots);
            }

            return dentistFreeTimeAvailability;
        }

        public async Task<bool> CheckEstimatedTimeIsExistedInFreeTiemAvailability(DateTime date)
        {
            return (await _dentistAvailabilityRepository.CheckDate(date)).Count > 0; 
        }

        public List<DentistAvailability> GetDentistAvailabilitysByDayOfWeek(DateTime date, int dentistId)
        {
           return _dentistAvailabilityRepository.GetByDayOfWeek(date, dentistId);
        }
    }
}
