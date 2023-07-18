using DentistBooking.Infrastructure;

namespace DentistBooking.API.ApiModels
{
    public class DentistAvailabilityModel
    {
        public int AvailabilityId { get; set; }
        public int? DentistId { get; set; }
        public string? DayOfWeek { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
    }
}
