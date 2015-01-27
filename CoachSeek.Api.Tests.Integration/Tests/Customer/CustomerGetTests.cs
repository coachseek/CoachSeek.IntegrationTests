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
        private const string FRED_FIRST_NAME = "Fred";
        private const string FLINTSTONE_LAST_NAME = "Flintstone";

        private Guid FredId { get; set; }
        private string FredEmail { get; set; }
        private string FredPhone { get; set; }

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
            RegisterFredCustomer();
            //RegisterBobbyLocation();
        }

        private void RegisterFredCustomer()
        {
            FredEmail = RandomEmail;
            FredPhone = RandomString;
            var json = CreateNewCustomerSaveCommand(FRED_FIRST_NAME, FLINTSTONE_LAST_NAME, FredEmail, FredPhone);
            var response = Post<CustomerData>(json);
            FredId = ((CustomerData)response.Payload).id;
        }

        //private void RegisterBobbyLocation()
        //{
        //    var json = CreateNewCoachSaveCommand(BOBBY_FIRST_NAME, SMITH_LAST_NAME, RandomEmail, RandomString);
        //    var response = Post<CoachData>(json);
        //    BobbyId = ((CoachData)response.Payload).id;
        //}

        private string CreateNewCustomerSaveCommand(string firstName, string lastName, string email, string phone)
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
            return Get<List<CoachData>>(url);
        }

        private Response WhenGetById(Guid customerId)
        {
            var url = BuildGetByIdUrl(customerId);
            return Get<CustomerData>(url);
        }


        //private void ThenReturnAllCoachesResponse(Response response)
        //{
        //    Assert.That(response, Is.Not.Null);
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        //    Assert.That(response.Payload, Is.Not.Null);
        //    var coaches = (List<CoachData>)response.Payload;
        //    Assert.That(coaches.Count, Is.EqualTo(2));

        //    var coachOne = coaches[0];
        //    Assert.That(coachOne.id, Is.EqualTo(AaronId));
        //    Assert.That(coachOne.firstName, Is.EqualTo(AARON_FIRST_NAME));
        //    Assert.That(coachOne.lastName, Is.EqualTo(SMITH_LAST_NAME));

        //    var coachTwo = coaches[1];
        //    Assert.That(coachTwo.id, Is.EqualTo(BobbyId));
        //    Assert.That(coachTwo.firstName, Is.EqualTo(BOBBY_FIRST_NAME));
        //    Assert.That(coachTwo.lastName, Is.EqualTo(SMITH_LAST_NAME));
        //}

        private void ThenReturnNotFoundResponse(Response response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        private void ThenReturnCustomerResponse(Response response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var customer = (CustomerData)response.Payload;
            Assert.That(customer.id, Is.EqualTo(FredId));
            Assert.That(customer.firstName, Is.EqualTo(FRED_FIRST_NAME));
            Assert.That(customer.lastName, Is.EqualTo(FLINTSTONE_LAST_NAME));
            Assert.That(customer.email, Is.EqualTo(FredEmail));
            Assert.That(customer.phone, Is.EqualTo(FredPhone.ToUpperInvariant()));
        }
    }
}
