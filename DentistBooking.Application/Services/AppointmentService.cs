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
    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;
        private readonly StaffRepository _StaffRepository;
        private readonly DentistAvailabilityRepository _dentistAvailabilityRepository;
        private readonly DentistRepository _dentistRepository;
        private readonly PatientRepository _patientRepository;
        private readonly TreatmentRepository _treatmentRepository;
        public AppointmentService(StaffRepository StaffRepository, AppointmentRepository appointmentRepository, PatientRepository patientRepository
            , DentistAvailabilityRepository dentistAvailabilityRepository, DentistRepository dentistRepository, TreatmentRepository treatmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _StaffRepository = StaffRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _dentistAvailabilityRepository = dentistAvailabilityRepository;
            _dentistRepository = dentistRepository;
            _treatmentRepository = treatmentRepository;
        }

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return _appointmentRepository.GetAllAppointments();
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsAsync();



                return appointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        public async Task<List<Appointment>> GetAppointmentsAsync(int pageSize, int pageNumber)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsAsync();

                var pagedAppointments = appointments
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return pagedAppointments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        public async Task<List<Appointment>> GetAppointmentsByDentistIdAsync(int dentistId, int pageSize, int pageNumber)
        {
            try
            {
                var appointments = await GetAppointmentsAsync(pageSize, pageNumber);
                var appointmentsByDentist = appointments.Where(a => a.StaffId == dentistId).ToList();

                return appointmentsByDentist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId, int pageSize, int pageNumber)
        {
            try
            {
                var appointments = await GetAppointmentsAsync(pageSize, pageNumber);
                var appointmentsByPatient = appointments.Where(a => a.PatientId == patientId).ToList();

                return appointmentsByPatient;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }


        public Appointment? GetAppointmentById(int id)
        {
            return _appointmentRepository.GetAppointmentsById(id);
        }

        public Appointment? GetAppointmentByStaffId(int id)
        {
            return _appointmentRepository.GetAppointmentsByStaffId(id);
        }

        public void CreateAppointment(Appointment appointment)
        {
            try
            {
                _appointmentRepository.Add(appointment);
                _appointmentRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

        }

        public Appointment UpdateAppointment(Appointment appointment)
        {
            var bookingTimeIsExisted = _appointmentRepository.GetAll().Where(a => a.Datetime == appointment.Datetime).ToList();
            if (bookingTimeIsExisted.Count > 0)
            {
                throw new Exception("Your appointment time is busy!!!");
            }

            //Check user is a new user or not. If he/she is a new user, he/she will have only 30 minutes
            var patientsIsExisted = _appointmentRepository.GetAll().Where(a => a.PatientId == appointment.PatientId).ToList();
            if (patientsIsExisted.Count == 0)
            {
                appointment.Duration = 30;
            }

            //Check dentist is existed or not
            var denstisIsExisted = _dentistAvailabilityRepository.GetAll().Where(d => d.DentistId == appointment.DentistId).FirstOrDefault();
            if (denstisIsExisted == null)
            {
                throw new Exception("Dentist is not existed!!!");
            }

            var checkDateTimeIsExistedInDentistWorkingTime = _dentistAvailabilityRepository.GetByDayOfWeek((DateTime)appointment.Datetime, (int) appointment.DentistId);
            if (checkDateTimeIsExistedInDentistWorkingTime.Count == 0)
            {
                throw new Exception($"Dentist is not working in {appointment.Datetime}!!!");
            }

            //Check dentist time is busy or not
            var listAppointmentOfDentis = _appointmentRepository.GetAll().Where(a => a.DentistId == appointment.DentistId && a.Datetime.Value.Date == appointment.Datetime.Value.Date).ToList();
            if (listAppointmentOfDentis.Count > 0)
            {
                TimeSpan timeStartAppointment = appointment.Datetime.Value.TimeOfDay;
                TimeSpan timeEndAppointment = timeStartAppointment + TimeSpan.FromHours(appointment.Duration.Value);
                foreach (var dentistAppointment in listAppointmentOfDentis)
                {
                    TimeSpan timeStart = dentistAppointment.Datetime.Value.TimeOfDay;
                    TimeSpan timeEnd = timeStart + TimeSpan.FromHours(dentistAppointment.Duration.Value);
                    if (timeStartAppointment >= timeStart && timeEndAppointment <= timeEnd)
                    {
                        throw new Exception("Dentist is busy now");
                    }
                    if (timeStartAppointment <= timeStart && timeEndAppointment <= timeEnd)
                    {
                        throw new Exception("Dentist is busy now");
                    }

                    if (timeStartAppointment >= timeStart && timeEndAppointment >= timeEnd)
                    {
                        throw new Exception("Dentist is busy now");
                    }
                }
            }
            var appointmentDentist = _dentistRepository.GetAll().Where(d => d.DentistId == appointment.DentistId).FirstOrDefault();
            //Check staff is existed or not
            var appointmentStaff = _StaffRepository.GetAll().Where(d => d.StaffId == appointment.StaffId).FirstOrDefault();
            if (appointmentStaff == null)
            {
                throw new Exception("Staff is not existed");
            }

            //Check patient is existed or not
            var appointmentPatient = _patientRepository.GetAll().Where(d => d.PatientId == appointment.PatientId).FirstOrDefault();
            if (appointmentPatient == null)
            {
                throw new Exception("Patient is not existed");
            }

            appointment.Staff = appointmentStaff;
            appointment.Patient = appointmentPatient;
            appointment.Dentist = appointmentDentist;
            _appointmentRepository.Update(appointment);
            _appointmentRepository.SaveChanges();
            return appointment;
        }

        public void DeleteAppointment(int id)
        {
            try
            {
                var appointment = _appointmentRepository.GetById(id);
                if (appointment != null)
                {
                    _appointmentRepository.Delete(appointment);
                    _appointmentRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public List<Appointment> GetAppointmentsByPatientEmail(string email)
        {
            return _appointmentRepository.GetAppointmentsByPatientEmail(email);
        }

        public List<Appointment> GetAppointmentsByDentistEmail(string dentistEmail)
        {
            return _appointmentRepository.GetAppointmentsByDentistEmail(dentistEmail);
        }

        public Appointment? CreateAnAppointment(Appointment appointment)
        {
            //Check all appointments need to book before 24 hours.
            //DateTime currentDateTime = DateTime.Now;
            /*DateTime minBookingDateTime = currentDateTime.AddHours(24);
            if (appointment.Datetime < minBookingDateTime)
            {
                throw new Exception("Appointment must be booked at least 24 hours");
            }*/
            //Check can not book greater than 2 appointment of one user in the same day.
            var appointmentList = _appointmentRepository.GetAll().Where(a => a.PatientId == appointment.PatientId && a.Datetime == appointment.Datetime).ToList();
            if (appointmentList.Count >= 2)
            {
                throw new Exception("Can not create two appointments in one days");
            }

            //Check your appointment is existed by another users or not
            var bookingTimeIsExisted = _appointmentRepository.GetAll().Where(a => a.Datetime == appointment.Datetime).ToList();
            if (bookingTimeIsExisted.Count > 0)
            {
                throw new Exception("Your appointment time is busy!!!");
            }

            //Check user is a new user or not. If he/she is a new user, he/she will have only 30 minutes
            var patientsIsExisted = _appointmentRepository.GetAll().Where(a => a.PatientId == appointment.PatientId).ToList();
            if (patientsIsExisted.Count == 0)
            {
                appointment.Duration = 0.5;
            }

            //Check dentist is existed or not
            var denstisIsExisted = _dentistAvailabilityRepository.GetAll().Where(d => d.DentistId == appointment.DentistId).FirstOrDefault();
            if (denstisIsExisted == null)
            {
                throw new Exception("Dentist is not existed!!!");
            }

            var checkDateTimeIsExistedInDentistWorkingTime = _dentistAvailabilityRepository.GetByDayOfWeek((DateTime)appointment.Datetime, (int)appointment.DentistId);
            if (checkDateTimeIsExistedInDentistWorkingTime.Count == 0)
            {
                throw new Exception($"Dentist is not working in {appointment.Datetime}!!!");
            }

            //Check dentist time is busy or not
            var listAppointmentOfDentis = _appointmentRepository.GetAll().Where(a => a.DentistId == appointment.DentistId && a.Datetime.Value.Date == appointment.Datetime.Value.Date).ToList();
            if (listAppointmentOfDentis.Count > 0)
            {
                TimeSpan timeStartAppointment = appointment.Datetime.Value.TimeOfDay;
                TimeSpan timeEndAppointment = timeStartAppointment.Add(TimeSpan.FromHours((double?)appointment.Duration ?? 0.5));
                foreach (var dentistAppointment in listAppointmentOfDentis)
                {
                    TimeSpan timeStart = dentistAppointment.Datetime.Value.TimeOfDay;
                    TimeSpan timeEnd = timeStart.Add(TimeSpan.FromHours((double) dentistAppointment.Duration));
                    if (timeStartAppointment >= timeStart && timeEndAppointment <= timeEnd)
                    {
                        throw new Exception("Dentist is busy now");
                    }
                    if (timeStartAppointment <= timeStart && timeEndAppointment <= timeEnd)
                    {
                        throw new Exception("Dentist is busy now");
                    }

                    if (timeStartAppointment >= timeStart && timeEndAppointment >= timeEnd)
                    {
                        throw new Exception("Dentist is busy now");
                    }
                }
            }
            var appointmentDentist = _dentistRepository.GetAll().Where(d => d.DentistId == appointment.DentistId).FirstOrDefault();
            //Check staff is existed or not
            var appointmentStaff = _StaffRepository.GetAll().Where(d => d.StaffId == appointment.StaffId).FirstOrDefault();
            if (appointmentStaff == null)
            {
                throw new Exception("Staff is not existed");
            }

            //Check patient is existed or not
            var appointmentPatient = _patientRepository.GetAll().Where(d => d.PatientId == appointment.PatientId).FirstOrDefault();
            if (appointmentPatient == null)
            {
                throw new Exception("Patient is not existed");
            }

            appointment.Status = AppointmentStatus.Scheduled;
            appointment.Staff = appointmentStaff;
            appointment.Patient = appointmentPatient;
            appointment.Dentist = appointmentDentist;
            _appointmentRepository.Add(appointment);
            _appointmentRepository.SaveChanges();
            return appointment;
        }


    }
}
