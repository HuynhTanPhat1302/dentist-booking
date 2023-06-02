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
        CreateMap<Treatment, TreatmentApiModel>();
        CreateMap<TreatmentApiModel, Treatment>();
        CreateMap<DentistAvailability, DentistAvailabilityApiModel>();
        CreateMap<DentistAvailabilityApiModel, DentistAvailability>();
    }
}
