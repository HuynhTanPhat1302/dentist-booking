namespace DentistBooking.API.ApiModels.DentistAvailability
{
    public class TimeSlot
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }


        public TimeSlot(TimeSpan startTime, TimeSpan endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
        public override string ToString()
        {
            return $"{StartTime:hh\\:mm\\:ss} - {EndTime:hh\\:mm\\:ss}";
        }
    }
}
