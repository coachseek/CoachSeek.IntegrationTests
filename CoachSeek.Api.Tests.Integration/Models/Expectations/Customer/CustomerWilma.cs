namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Customer
{
    public class CustomerWilma : ExpectedCustomer
    {
        public override string FirstName { get { return "Wilma"; } }
        public override string LastName { get { return "Flintstone"; } }


        public CustomerWilma()
            : base(null, Random.RandomString)
        { }
    }
}
