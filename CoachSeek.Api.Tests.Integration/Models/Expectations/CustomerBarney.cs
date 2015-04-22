namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class CustomerBarney : ExpectedCustomer
    {
        public override string FirstName { get { return "Barney"; } }
        public override string LastName { get { return "Rubble"; } }
    }
}
