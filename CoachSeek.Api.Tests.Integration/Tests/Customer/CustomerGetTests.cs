using System;
using System.Collections.Generic;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    [TestFixture]
    public class CustomerGetTests : WebIntegrationTest
    {
        private const string BAMBAM_FIRST_NAME = "BamBam";
        private const string FRED_FIRST_NAME = "Fred";
        private const string WILMA_FIRST_NAME = "Wilma";
        private const string RUBBLE_LAST_NAME = "Rubble";
        private const string FLINTSTONE_LAST_NAME = "Flintstone";

        private Guid BambamId { get; set; }
        private Guid FredId { get; set; }
        private Guid WilmaId { get; set; }
        private string FredEmail { get; set; }
        private string FredPhone { get; set; }
        private string BambamPhone { get; set; }

        protected override string RelativePath
        {
            get { return "Customers"; }
        }


        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestCustomers();
        }

        private void RegisterTestCustomers()
        {
            RegisterBamBamRubbleCustomer();
            RegisterFredFlintstoneCustomer();
            RegisterWilmaFlintstoneCustomer();
        }

        private void RegisterFredFlintstoneCustomer()
        {
            FredEmail = Random.RandomEmail;
            FredPhone = Random.RandomString;
            var json = CreateNewCustomerSaveCommand(FRED_FIRST_NAME, FLINTSTONE_LAST_NAME, FredEmail, FredPhone);
            var response = Post<CustomerData>(json);
            FredId = ((CustomerData)response.Payload).id;
        }

        private void RegisterWilmaFlintstoneCustomer()
        {
            var json = CreateNewCustomerSaveCommand(WILMA_FIRST_NAME, FLINTSTONE_LAST_NAME);
            var response = Post<CustomerData>(json);
            WilmaId = ((CustomerData)response.Payload).id;
        }

        private void RegisterBamBamRubbleCustomer()
        {
            BambamPhone = "021 666 666";
            var json = CreateNewCustomerSaveCommand(BAMBAM_FIRST_NAME, RUBBLE_LAST_NAME, null, BambamPhone);
            var response = Post<CustomerData>(json);
            BambamId = ((CustomerData)response.Payload).id;
        }


        private string CreateNewCustomerSaveCommand(string firstName, string lastName, string email = null, string phone = null)
        {
            var coach = new ApiCustomerSaveCommand
            {
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone
            };

            return JsonConvert.SerializeObject(coach);
        }


        [Test]
        public void WhenGetAll_ThenReturnAllCoachesResponse()
        {
            var response = WhenGetAll();
            ThenReturnAllCoachesResponse(response);
        }

        [Test]
        public void GivenInvalidCustomerId_WhenGetById_ThenReturnNotFoundResponse()
        {
            var id = GivenInvalidCustomerId();
            var response = WhenGetById(id);
            ThenReturnNotFoundResponse(response);
        }

        [Test]
        public void GivenValidCustomerId_WhenGetById_ThenReturnCustomerResponse()
        {
            var id = GivenValidCustomerId();
            var response = WhenGetById(id);
            ThenReturnCustomerResponse(response);
        }


        private Guid GivenInvalidCustomerId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidCustomerId()
        {
            return FredId;
        }


        private Response WhenGetAll()
        {
            var url = BuildGetAllUrl();
            return AuthenticatedGet<List<CustomerData>>(url);
        }

        private Response WhenGetById(Guid customerId)
        {
            var url = BuildGetByIdUrl(customerId);
            return AuthenticatedGet<CustomerData>(url);
        }


        private void ThenReturnAllCoachesResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var coaches = (List<CustomerData>)response.Payload;
            Assert.That(coaches.Count, Is.EqualTo(3));

            var coachOne = coaches[0];
            Assert.That(coachOne.id, Is.EqualTo(FredId));
            Assert.That(coachOne.firstName, Is.EqualTo(FRED_FIRST_NAME));
            Assert.That(coachOne.lastName, Is.EqualTo(FLINTSTONE_LAST_NAME));
            Assert.That(coachOne.email, Is.EqualTo(FredEmail));
            Assert.That(coachOne.phone, Is.EqualTo(FredPhone.ToUpper()));

            var coachTwo = coaches[1];
            Assert.That(coachTwo.id, Is.EqualTo(WilmaId));
            Assert.That(coachTwo.firstName, Is.EqualTo(WILMA_FIRST_NAME));
            Assert.That(coachTwo.lastName, Is.EqualTo(FLINTSTONE_LAST_NAME));
            Assert.That(coachTwo.email, Is.Null);
            Assert.That(coachTwo.phone, Is.Null);

            var coachThree = coaches[2];
            Assert.That(coachThree.id, Is.EqualTo(BambamId));
            Assert.That(coachThree.firstName, Is.EqualTo(BAMBAM_FIRST_NAME));
            Assert.That(coachThree.lastName, Is.EqualTo(RUBBLE_LAST_NAME));
            Assert.That(coachThree.email, Is.Null);
            Assert.That(coachThree.phone, Is.EqualTo(BambamPhone));

        }

        private void ThenReturnNotFoundResponse(Response response)
        {
            AssertNotFound(response);
        }

        private void ThenReturnCustomerResponse(Response response)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.EqualTo(FredId));
            Assert.That(customer.firstName, Is.EqualTo(FRED_FIRST_NAME));
            Assert.That(customer.lastName, Is.EqualTo(FLINTSTONE_LAST_NAME));
            Assert.That(customer.email, Is.EqualTo(FredEmail));
            Assert.That(customer.phone, Is.EqualTo(FredPhone.ToUpperInvariant()));
        }
    }
}
