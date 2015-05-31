using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Business
{
    public abstract class BusinessTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "Business"; }
        }


        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
        }
    }
}
