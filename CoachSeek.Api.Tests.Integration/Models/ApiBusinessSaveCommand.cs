namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiBusinessSaveCommand
    {
        // Business Id is not needed because we can get that from the authentication context.

        public string name { get; set; }
        public ApiBusinessPaymentOptions payment { get; set; }
    }
}
