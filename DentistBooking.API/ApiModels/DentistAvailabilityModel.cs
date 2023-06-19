using DentistBooking.Infrastructure;

namespace DentistBooking.API.ApiModels
{
    public class DentistAvailabilityModel
    {
        public int AvailabilityId { get; set; }
        public int? DentistId { get; set; }
        public string? DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
