namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiRepetition
    {
        public int sessionCount { get; set; }
        public string repeatFrequency { get; set; }

        public ApiRepetition()
        {
            sessionCount = 1;
            repeatFrequency = null;
        }

        public ApiRepetition(int sessionCount, string repeatFrequency)
        {
            this.sessionCount = sessionCount;
            this.repeatFrequency = repeatFrequency;
        }
    }
}
