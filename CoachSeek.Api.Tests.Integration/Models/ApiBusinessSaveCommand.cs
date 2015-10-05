namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiBusinessSaveCommand
    {
        // Business Id is not needed because we can get that from the authentication context.

        public string name { get; set; }
        public string domain { get; set; }
        public string sport { get; set; }
        public ApiBusinessPaymentOptions payment { get; set; }
    }
}
