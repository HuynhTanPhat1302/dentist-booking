﻿using AutoMapper;
using DentistBooking.API.ApiModels;
using DentistBooking.API.ApiModels.DentistBooking.API.ApiModels;
using DentistBooking.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Patient, PatientApiModel>();
        CreateMap<PatientApiModel, Patient>();
        CreateMap<staff, StaffApiModel>();
        CreateMap<StaffApiModel, staff>();
        CreateMap<ProposeAppointment, ProposeAppointmentRequestModel>();
        CreateMap<ProposeAppointmentRequestModel, ProposeAppointment>();
        CreateMap<Treatment, TreatmentApiRequestModel>();
        CreateMap<TreatmentApiRequestModel, Treatment>();
        CreateMap<MedicalRecord, MedicalRecordApiRequestModel>();
        CreateMap<MedicalRecordApiRequestModel, MedicalRecord>();
        CreateMap<MedicalRecord, MedicalRecordApiRequestModel>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientName : null))
            .ForMember(dest => dest.DentistName, opt => opt.MapFrom(src => src.Dentist != null ? src.Dentist.DentistName : null))
            .ForMember(dest => dest.IllnessName, opt => opt.MapFrom(src => src.Illness != null ? src.Illness.IllnessName : null))
            .ForMember(dest => dest.TreatmentName, opt => opt.MapFrom(src => src.Treatment != null ? src.Treatment.TreatmentName : null));
        CreateMap<Appointment, AppointmentApiRequestModel>();
        CreateMap<AppointmentApiRequestModel, Appointment>();
        CreateMap<Appointment, AppointmentApiRequestModel>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientName : null))
            .ForMember(dest => dest.DentistName, opt => opt.MapFrom(src => src.Dentist != null ? src.Dentist.DentistName : null))
            .ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.Staff != null ? src.Staff.StaffName : null));
           




    }
}
