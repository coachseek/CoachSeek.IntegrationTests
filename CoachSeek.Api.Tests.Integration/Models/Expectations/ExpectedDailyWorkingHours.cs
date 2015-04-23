namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class ExpectedDailyWorkingHours
    {
        public bool IsAvailable { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }

        public ExpectedDailyWorkingHours()
        { }

        public ExpectedDailyWorkingHours(bool isAvailable, string startTime, string finishTime)
        {
            IsAvailable = isAvailable;
            StartTime = startTime;
            FinishTime = finishTime;
        }


        public ApiDailyWorkingHours ToApi()
        {
            return new ApiDailyWorkingHours(IsAvailable, StartTime, FinishTime);
        }
    }
}
