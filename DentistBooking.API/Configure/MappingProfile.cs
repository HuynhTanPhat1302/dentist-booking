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

        CreateMap<Patient, PatientRespondModel>();
        CreateMap<PatientRespondModel, Patient>();

        CreateMap<staff, StaffApiModel>();
        CreateMap<StaffApiModel, staff>();

        CreateMap<staff, StaffRespondModel>();
        CreateMap<StaffRespondModel, staff>();

        CreateMap<ProposeAppointment, ProposeAppointmentRequestModel>();
        CreateMap<ProposeAppointmentRequestModel, ProposeAppointment>();

        CreateMap<ProposeAppointment, ProposeAppointmentRespondModel>()
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        CreateMap<ProposeAppointmentRespondModel, ProposeAppointment>();

        CreateMap<ProposeAppointment, ProposeAppointmentStatusRequestModel>()
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        CreateMap<ProposeAppointmentStatusRequestModel, ProposeAppointment>();

        CreateMap<ProposeAppointment, ProposeAppointmentUpdateModel>();
        CreateMap<ProposeAppointmentUpdateModel, ProposeAppointment>()
    .ForAllMembers(opts =>
    {
        opts.Condition((src, dest, srcMember) =>
            (srcMember != null || opts.DestinationMember.Name == "PatientId"));
    });

        CreateMap<Treatment, TreatmentApiRequestModel>();
        CreateMap<TreatmentApiRequestModel, Treatment>();

        CreateMap<Treatment, TreatmentRequestModel>();
        CreateMap<TreatmentRequestModel, Treatment>();

        CreateMap<Treatment, TreatmentRespondModel>();
        CreateMap<TreatmentRespondModel, Treatment>();

        CreateMap<MedicalRecord, MedicalRecordApiRequestModel>();
        CreateMap<MedicalRecordApiRequestModel, MedicalRecord>();
        CreateMap<MedicalRecord, MedicalRecordApiRequestModel>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientName : null))
            .ForMember(dest => dest.DentistName, opt => opt.MapFrom(src => src.Dentist != null ? src.Dentist.DentistName : null))
            .ForMember(dest => dest.IllnessName, opt => opt.MapFrom(src => src.Illness != null ? src.Illness.IllnessName : null))
            .ForMember(dest => dest.TreatmentName, opt => opt.MapFrom(src => src.Treatment != null ? src.Treatment.TreatmentName : null));

        CreateMap<MedicalRecord, MedicalRecordCreatedModel>();
        CreateMap<MedicalRecordCreatedModel, MedicalRecord>();

        CreateMap<MedicalRecord, MedicalRecordUpdatedModel>();
        CreateMap<MedicalRecordUpdatedModel, MedicalRecord>();

        CreateMap<MedicalRecord, MedicalRecordtStatusRequestModel>()
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        CreateMap<MedicalRecordtStatusRequestModel, MedicalRecord>();

        CreateMap<Appointment, AppointmentApiRequestModel>();
        CreateMap<AppointmentApiRequestModel, Appointment>();
        CreateMap<Appointment, AppointmentApiRequestModel>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientName : null))
            .ForMember(dest => dest.DentistName, opt => opt.MapFrom(src => src.Dentist != null ? src.Dentist.DentistName : null))
            .ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.Staff != null ? src.Staff.StaffName : null));

        CreateMap<Appointment, AppointmentApiModelRequest>();
        CreateMap<AppointmentApiModelRequest, Appointment>();
        CreateMap<Appointment, AppointmentApiModel>()
            .ForMember(m => m.DentistName, opt => opt.MapFrom(src => src.Dentist.DentistName))
            .ForMember(m => m.StaffName, opt => opt.MapFrom(src => src.Staff.StaffName))
            .ForMember(m => m.PatientName, opt => opt.MapFrom(src => src.Patient.PatientName));

        CreateMap<Appointment, AppointmentStatusRequestModel>()
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        CreateMap<AppointmentStatusRequestModel, Appointment>();

        CreateMap<AppointmentApiModel, Appointment>();

        CreateMap<AppointmentRespondModel, Appointment>();
        CreateMap<Appointment, AppointmentRespondModel>();

        CreateMap<AppointmentCreateModel, Appointment>();
        CreateMap<Appointment, AppointmentCreateModel>();

        CreateMap<AppointmentUpdateModel, Appointment>();
        CreateMap<Appointment, AppointmentUpdateModel>();


        CreateMap<Patient, PatientApiRequestModel>();
        CreateMap<PatientApiRequestModel, Patient>();

        CreateMap<DentistAccountApiModel, Dentist>();
        CreateMap<Dentist, DentistAccountApiModel>();
        CreateMap<Patient, RegisterRequestModel>();
        CreateMap<RegisterRequestModel, Patient>();

        CreateMap<Illness, IllnessRespondModel>();
        CreateMap<IllnessRespondModel, Illness>();

        CreateMap<Illness, IllnessRequestModel>();
        CreateMap<IllnessRequestModel, Illness>();

        CreateMap<MedicalRecord, NestedMedicalRecordRespondModel>()
            .ForMember(dest => dest.Dentist, opt => opt.MapFrom(src => src.Dentist))
            .ForMember(dest => dest.Illness, opt => opt.MapFrom(src => src.Illness))
            .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient))
            .ForMember(dest => dest.Treatment, opt => opt.MapFrom(src => src.Treatment))
            .ForMember(dest => dest.AppointmentDetails, opt => opt.MapFrom(src => src.AppointmentDetails));

        CreateMap<NestedMedicalRecordRespondModel, MedicalRecord>();


        CreateMap<DentistApiModel, Dentist>();
        CreateMap<Dentist, DentistApiModel>();

        CreateMap<Dentist, DentistRepondModel>();
        CreateMap<DentistRepondModel, Dentist>();


        CreateMap<DentistAvailabilityRequestModel, DentistAvailability>()
            .ForMember(m => m.StartTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.StartTime)))
            .ForMember(m => m.EndTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.EndTime)));
        CreateMap<DentistAvailability, DentistAvailabilityRequestModel>();

        CreateMap<DentistAvailabilityModel, DentistAvailability>();
        CreateMap<DentistAvailability, DentistAvailabilityModel>();

        CreateMap<AppointmentDetail, AppointmentDetailRespondModel>();
        CreateMap<AppointmentDetailRespondModel, AppointmentDetail>();

        CreateMap<AppointmentDetail, AppointmentDetailCreateModel>();
        CreateMap<AppointmentDetailCreateModel, AppointmentDetail>();

    }
}
