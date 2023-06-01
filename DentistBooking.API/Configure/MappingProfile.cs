using AutoMapper;
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
        CreateMap<MedicalRecord, MedicalRecordsApiModel>();
        CreateMap<MedicalRecordsApiModel, MedicalRecord>();
        CreateMap<Appointment, AppointmentApiModelRequest>();
        CreateMap<AppointmentApiModelRequest, Appointment>();
        CreateMap<Appointment, AppointmentApiModel>();
        CreateMap<AppointmentApiModel, Appointment>();
        CreateMap<Patient, PatientApiModelRequest>();
        CreateMap<PatientApiModelRequest, Patient>();
    }
}
