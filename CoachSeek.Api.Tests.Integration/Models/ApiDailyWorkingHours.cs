namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiDailyWorkingHours
    {
        public bool isAvailable { get; set; }
        public string startTime { get; set; }
        public string finishTime { get; set; }

        public ApiDailyWorkingHours()
        { }

        public ApiDailyWorkingHours(bool isAvailable, string startTime, string finishTime)
        {
            this.isAvailable = isAvailable;
            this.startTime = startTime;
            this.finishTime = finishTime;
        }
    }
}
