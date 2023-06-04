using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DentistBooking.Application.Services
{
    public class StaffService : IStaffService
    {
        private readonly StaffRepository _StaffRepository;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly PatientRepository _patientRepository;
        private readonly MedicalRepository _medicalRepository;
        private readonly DentistAvailabilityRepository _dentistAvailabilityRepository;
        private readonly DentistRepository _dentistRepository;
        private readonly TreatmentRepository _treatmentRepository;
        private readonly IllnessRepository _illnessRepository;


        public StaffService(StaffRepository StaffRepository, AppointmentRepository appointmentRepository, PatientRepository patientRepository
            , MedicalRepository medicalRepository, DentistAvailabilityRepository dentistAvailabilityRepository, DentistRepository dentistRepository
            , TreatmentRepository treatmentRepository, IllnessRepository illnessRepository)
        {
            _StaffRepository = StaffRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _medicalRepository = medicalRepository;
            _dentistAvailabilityRepository = dentistAvailabilityRepository;
            _dentistRepository = dentistRepository;
            _treatmentRepository = treatmentRepository;
            _illnessRepository = illnessRepository;
        }

        public IEnumerable<staff> GetAllStaffs()
        {
            return _StaffRepository.GetAll();
        }

        public staff GetStaffById(int id)
        {
            return _StaffRepository.GetById(id);
        }

        public void CreateStaff(staff Staff)
        {
            _StaffRepository.Add(Staff);
            _StaffRepository.SaveChanges();
        }

        public void UpdateStaff(staff Staff)
        {
            _StaffRepository.Update(Staff);
            _StaffRepository.SaveChanges();
        }

        public void DeleteStaff(int id)
        {
            var Staff = _StaffRepository.GetById(id);
            if (Staff != null)
            {
                _StaffRepository.Delete(Staff);
                _StaffRepository.SaveChanges();
            }
        }

        public staff? GetStaffByEmail(string email)
        {
            return _StaffRepository.GetStaffByEmail(email);
        }

        public Patient? CreateAccountOfPatient(Patient patient)
        {
            var emailIsExisted = _patientRepository.GetAll().Where(p => p.Email.Equals(patient.Email)).FirstOrDefault();
            if (emailIsExisted != null)
            {
                throw new Exception("Email is existed");
            }

            var patientCodeIsExisted = _patientRepository.GetAll().Where(p => p.PatientCode.Equals(patient.PatientCode)).FirstOrDefault(); 
            if (patientCodeIsExisted != null)
            {
                throw new Exception("Patient code is existed");
            }
            _patientRepository.Add(patient);
            _patientRepository.SaveChanges();
            return patient;
        }

        public Appointment? CreateAnAppointment(Appointment appointment)
        {
            //Check all appointments need to book before 24 hours.
            DateTime currentDateTime = DateTime.Now;
            DateTime minBookingDateTime = currentDateTime.AddHours(24);
            if (appointment.Datetime < minBookingDateTime)
            {
                throw new Exception("Appointment must be booked at least 24 hours");
            }

            //Check all appointments need to book under one month.
            DateTime currentDate = DateTime.Today;
            DateTime maxBookingDate = currentDate.AddMonths(1);
            if (appointment.Datetime > maxBookingDate)
            {
                throw new Exception("Not book an appoint far more than a month");
            }

            //Check can not book greater than 2 appointment of one user in the same day.
            var appointmentList = _appointmentRepository.GetAll().Where(a => a.PatientId == appointment.PatientId && a.Datetime == appointment.Datetime).ToList();
            if (appointmentList.Count >= 2)
            {
                throw new Exception("Can not create two appointments in one days");
            }

            //Check one appointment has a duration under 3 hours.
            if (appointment.Duration > 3)
            {
                throw new Exception("Duration appointment can not be large than three hour");
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
                appointment.Duration = 30;
            }

            //Check dentist is existed or not
            var denstisIsExisted = _dentistAvailabilityRepository.GetAll().Where(d => d.DentistId == appointment.DentistId).FirstOrDefault();
            if (denstisIsExisted == null)
            {
                throw new Exception("Dentist is not existed!!!");
            }

            //Check dentist time is busy or not
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
            _appointmentRepository.Add(appointment);
            _appointmentRepository.SaveChanges();
            return appointment;
        }



        public List<Patient> GetAllPatients()
        {
            return _patientRepository.GetAllPatients();
        }

        public MedicalRecord? GetMedicalRecords(int id)
        {
            MedicalRecord  res =_medicalRepository.GetMedicalById(id);
            return res;
        }

        public List<MedicalRecord> GetMedicalRecordsOfPatient(int patientId)
        {
            return _medicalRepository.GetMedicalRecordByPatientId(patientId);
        }

        public Patient? GetPatient(int id)
        {
            return _patientRepository.FindById(id);
        }

        public Appointment? GetAppointment(int id)
        {
            return _appointmentRepository.GetAppointmentsById(id);
        }

        public List<Appointment> GetAllAppointments()
        {
            return _appointmentRepository.GetAllAppointments();
        }
    }
}
