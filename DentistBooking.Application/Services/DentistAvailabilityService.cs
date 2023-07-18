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

        /*public async Task<Dictionary<string, (TimeSpan Start, TimeSpan End)>> GetDentistFreetimeAvailability(DateTime dateRequest)
        {
            Dictionary<string, (TimeSpan Start, TimeSpan End)> dentistFreeTimeAvailability = new Dictionary<string, (TimeSpan Start, TimeSpan End)>();
            var listDentistAvailability = await _dentistAvailabilityRepository.GetByDayOfWeekAsync(dateRequest);
            if (listDentistAvailability.Count == 0)
            {
                throw new Exception($"No dentist working in {dateRequest}");
            }

            int index = 0;
            foreach (var dentistAvailability in listDentistAvailability)
            {
                var listDentistAppointment = await _appointmentRepository.GetAppointmentsByDentistIdAndDateTimeAsync((int)dentistAvailability.DentistId, dateRequest);

                if (listDentistAppointment.Count() == 0)
                {
                    if (dentistAvailability.DayOfWeek.Equals(dateRequest.DayOfWeek.ToString()))
                    {
                        dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{index + 1}", (dentistAvailability.StartTime.Value, dentistAvailability.EndTime.Value));
                    }
                    else
                    {
                        throw new Exception($"No dentist working in {dateRequest}");
                    }
                }
                for (int i = 0; i < listDentistAppointment.Count; i++)
                {
                    if (dentistAvailability.DentistId == listDentistAppointment[i].DentistId)
                    {
                        var endTimeAppointment = listDentistAppointment[i].Datetime.Value.Add(TimeSpan.FromHours((double)listDentistAppointment[i].Duration));
                        if (listDentistAppointment[i].Datetime.Value.TimeOfDay > dentistAvailability.StartTime && endTimeAppointment.TimeOfDay <= dentistAvailability.EndTime)
                        {

                            if (i + 1 < listDentistAppointment.Count)
                            {
                                if (dentistFreeTimeAvailability.ContainsKey(dentistAvailability.Dentist.DentistName + $" #{i}"))
                                {
                                    var nextDentistAvailability = listDentistAppointment[i + 1];
                                    TimeSpan nextEndTimeDate = nextDentistAvailability.Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)nextDentistAvailability.Duration));
                                    var currentDentistAvailability = listDentistAppointment[i];

                                    DateTime currentEndTimeDate = currentDentistAvailability.Datetime.Value.Add(TimeSpan.FromHours((double)currentDentistAvailability.Duration));
                                    var currentEndTime = currentEndTimeDate.TimeOfDay;
                                    var prevDentistAvailability = listDentistAppointment[i - 1];
                                    TimeSpan preEndTime = prevDentistAvailability.Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)prevDentistAvailability.Duration));
                                    if (preEndTime.Equals(currentDentistAvailability.Datetime.Value.TimeOfDay) && nextDentistAvailability == null)
                                    {
                                        dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (preEndTime, dentistAvailability.EndTime.Value));
                                    }
                                    else if (preEndTime.Equals(currentDentistAvailability.Datetime.Value.TimeOfDay) == true && nextDentistAvailability != null)
                                    {
                                        dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (currentEndTime, nextDentistAvailability.Datetime.Value.TimeOfDay));

                                    }
                                    else if (preEndTime.Equals(currentDentistAvailability.Datetime.Value.TimeOfDay) == false && nextDentistAvailability != null)
                                    {
                                        if (dentistFreeTimeAvailability.ContainsValue((preEndTime, currentDentistAvailability.Datetime.Value.TimeOfDay)))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (preEndTime, currentDentistAvailability.Datetime.Value.TimeOfDay));
                                        }

                                    }
                                }
                                else
                                {
                                    TimeSpan endTime = listDentistAppointment[i].Datetime.Value.TimeOfDay;
                                    dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (dentistAvailability.StartTime.Value, endTime));

                                }
                            }
                            else if (i + 1 == listDentistAppointment.Count)
                            {
                                if (listDentistAppointment.Count == 1)
                                {
                                    if (listDentistAppointment[i].Datetime.Value.TimeOfDay == dentistAvailability.StartTime)
                                    {
                                        TimeSpan curendTime = listDentistAppointment[i].Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)listDentistAppointment[i].Duration));
                                        dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (curendTime, dentistAvailability.EndTime.Value));
                                    }
                                    else
                                    {
                                        TimeSpan endTime = listDentistAppointment[i].Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)listDentistAppointment[i].Duration));
                                        dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (dentistAvailability.StartTime.Value, listDentistAppointment[i].Datetime.Value.TimeOfDay));
                                        dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 2}", (endTime, dentistAvailability.EndTime.Value));
                                    }

                                    break;
                                }
                                var currentDentistAvailability = listDentistAppointment[i];
                                TimeSpan currentEndTime = currentDentistAvailability.Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)currentDentistAvailability.Duration));
                                var prevDentistAvailability = listDentistAppointment[i - 1];
                                TimeSpan preEndTime = prevDentistAvailability.Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)prevDentistAvailability.Duration));
                                if (preEndTime.Equals(currentDentistAvailability.Datetime.Value.TimeOfDay))
                                {
                                    if (currentEndTime.Equals(dentistAvailability.EndTime.Value))
                                    {
                                        break;
                                    }
                                    dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (currentEndTime, dentistAvailability.EndTime.Value));
                                }
                                else
                                {
                                    if (dentistFreeTimeAvailability.ContainsValue((preEndTime, currentDentistAvailability.Datetime.Value.TimeOfDay)))
                                    {
                                        if (currentEndTime.Equals(dentistAvailability.EndTime.Value))
                                        {
                                            break;
                                        }
                                        dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (currentEndTime, dentistAvailability.EndTime.Value));
                                        break;
                                    }

                                    dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (preEndTime, currentDentistAvailability.Datetime.Value.TimeOfDay));
                                    if (currentEndTime.Equals(dentistAvailability.EndTime.Value))
                                    {
                                        break;
                                    }
                                    dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 2}", (currentEndTime, dentistAvailability.EndTime.Value));
                                }
                            }
                        }
                        else if (listDentistAppointment[i].Datetime.Value.TimeOfDay == dentistAvailability.StartTime && endTimeAppointment.TimeOfDay < dentistAvailability.EndTime)
                        {
                            if (i + 1 < listDentistAppointment.Count)
                            {
                                var nextDentistAvailability = listDentistAppointment[i + 1];
                                TimeSpan nextEndTimeDate = nextDentistAvailability.Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)nextDentistAvailability.Duration));
                                var currentDentistAvailability = listDentistAppointment[i];

                                DateTime currentEndTimeDate = currentDentistAvailability.Datetime.Value.Add(TimeSpan.FromHours((double)currentDentistAvailability.Duration));
                                var currentEndTime = currentEndTimeDate.TimeOfDay;

                                TimeSpan endTime = listDentistAppointment[i].Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)listDentistAppointment[i].Duration));
                                if (endTime.Equals(nextEndTimeDate))
                                {
                                    dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (endTime, nextEndTimeDate));
                                }
                                if (nextDentistAvailability == null)
                                {
                                    dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (endTime, dentistAvailability.EndTime.Value));
                                }
                            }
                            else
                            {
                                TimeSpan endTime = listDentistAppointment[i].Datetime.Value.TimeOfDay.Add(TimeSpan.FromHours((double)listDentistAppointment[i].Duration));
                                dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" #{i + 1}", (endTime, dentistAvailability.EndTime.Value));
                            }
                        }
                    }
                }
                index++;
            }
            *//*if (dentistFreeTimeAvailability.Count > 0)
            {
                int newIndex = 0;
                int slot = 1;
                foreach (var dentistAvailability in listDentistAvailability)
                {
                    var listAppointment = await _appointmentRepository.CheckAppointmentIsExistedInWorking((int)dentistAvailability.DentistId, (TimeSpan)dentistAvailability.StartTime, (TimeSpan)dentistAvailability.EndTime);
                    if (listAppointment.Count == 0)
                    {
                        dentistFreeTimeAvailability.Add(dentistAvailability.Dentist.DentistName + $" slot {slot} #{newIndex}", (dentistAvailability.StartTime.Value, dentistAvailability.EndTime.Value));
                    }
                    newIndex++;
                    slot++;
                }
            }*//*

            return dentistFreeTimeAvailability;
        }*/
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
    }
}
