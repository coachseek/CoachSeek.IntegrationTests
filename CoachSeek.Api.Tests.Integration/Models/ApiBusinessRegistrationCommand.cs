namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiBusinessRegistrationCommand
    {
        public string businessName { get; set; }
        public ApiBusinessRegistrant registrant { get; set; }
    }
}
