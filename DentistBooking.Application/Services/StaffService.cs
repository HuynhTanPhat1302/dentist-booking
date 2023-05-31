using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Utils;
using DentistBooking.Infrastructure;
using DentistBooking.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Services
{
    public class StaffService : IStaffService
    {
        private readonly AppointmentRepository _appointmentRepository;
        private readonly PatientRepository _patientRepository;
        private readonly MedicalRepository _medicalRepository;
        private readonly DentistAvailabilityRepository _dentistAvailabilityRepository;
        private readonly IDentistService _dentistService;

        public StaffService(AppointmentRepository appointmentRepository, PatientRepository patientRepository
            , MedicalRepository medicalRepository, DentistAvailabilityRepository dentistAvailabilityRepository, IDentistService dentistService) 
        { 
            _appointmentRepository= appointmentRepository;
            _patientRepository= patientRepository;
            _medicalRepository = medicalRepository;
            _dentistAvailabilityRepository= dentistAvailabilityRepository;
            _dentistService= dentistService;
        }
        public Patient CreateAccountOfPatient(Patient patient)
        {
            var emailIsExisted = _patientRepository.GetAll().Where(p => p.Email.Equals(patient.Email)).ToList();
            if (emailIsExisted == null)
            {
                throw new Exception("Email is existed");
            }
            _patientRepository.Add(patient);
            _patientRepository.SaveChanges();
            return patient;
        }

        public Appointment CreateAnAppointment(Appointment appointment)
        {
            var bookingTimeIsExisted = _appointmentRepository.GetAll().Where(a => a.Datetime == appointment.Datetime).ToList();
            if (bookingTimeIsExisted.Count > 0)
            {
                throw new Exception("Your appointment time is busy!!!");
            }
            var patientsIsExisted = _appointmentRepository.GetAll().Where(a => a.PatientId == appointment.PatientId).ToList();
            if (patientsIsExisted != null)
            {
                appointment.Duration = 30;
            }
            var denstisIsExisted = _dentistAvailabilityRepository.GetAll().Where(d => d.DentistId == appointment.DentistId).FirstOrDefault();
            if (denstisIsExisted == null)
            {
                throw new Exception("Dentist is not existed!!!");
            }
            var listAppointmentOfDentis = _appointmentRepository.GetAll().Where(a => a.DentistId == appointment.DentistId && a.Datetime.Value.Date == appointment.Datetime.Value.Date).ToList();
            if (listAppointmentOfDentis.Count > 0)
            {
                TimeSpan timeStartAppointment = appointment.Datetime.Value.TimeOfDay;
                TimeSpan timeEndAppointment = timeStartAppointment + TimeSpan.FromMinutes(appointment.Duration.Value);
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
           /* List<TimeSlot> timeSlotList = _dentistService.GetAvailableTimeSlots(denstisIsExisted, listAppointMent, appointment.Datetime.Value);
            TimeSpan timeAppointed = appointment.Datetime.Value.TimeOfDay + TimeSpan.FromMinutes(appointment.Duration.Value);
            TimeSlot timeCheck = new TimeSlot();
            timeCheck.StartTime = appointment.Datetime.Value.TimeOfDay;
            timeCheck.EndTime = timeAppointed;
            foreach(var timeSlot in timeSlotList )
            {
                if (timeSlot == timeCheck)
                {
                    throw new Exception("Dentist is busy now");
                }
            }*/
            _appointmentRepository.Add(appointment);
            _appointmentRepository.SaveChanges();
            return appointment;
        }



        public List<Patient> GetAllPatients()
        {
            return _patientRepository.GetAll().ToList();
        }

        public List<MedicalRecord> GetMedicalRecords(int id)
        {
            return _medicalRepository.GetAll().Where(m => m.PatientId == id).ToList();
        }

        public Patient GetPatient(int id)
        {
            return _patientRepository.GetById(id);
        }
    }
}
