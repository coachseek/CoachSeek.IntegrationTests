using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    public abstract class ServiceTests : WebIntegrationTest
    {
        private const string MINI_RED_NAME = "Mini Red";
        private const string MINI_RED_DESCRIPTION = "Mini Red Service";
        private const string MINI_BLUE_NAME = "Mini Blue";

        private Guid MiniRedId { get; set; }
        private Guid MiniBlueId { get; set; }
        private string NewServiceName { get; set; }
        private string NewServiceDescription { get; set; }
        private int? Duration { get; set; }
        private string Colour { get; set; }


        protected override string RelativePath
        {
            get { return "Services"; }
        }

        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestServices();

            NewServiceName = string.Empty;
        }

        private void RegisterTestServices()
        {
            RegisterMiniRedService();
            RegisterMiniBlueService();
        }

        private void RegisterMiniRedService()
        {
            var json = CreateNewServiceSaveCommand(MINI_RED_NAME, MINI_RED_DESCRIPTION);
            var response = Post<ServiceData>(json);
            MiniRedId = ((ServiceData)response.Payload).id;
        }

        private void RegisterMiniBlueService()
        {
            var json = CreateNewServiceSaveCommand(MINI_BLUE_NAME, "Mini Blue Description");
            var response = Post<ServiceData>(json);
            MiniBlueId = ((ServiceData)response.Payload).id;
        }

        private string CreateNewServiceSaveCommand(string name, string description)
        {
            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                name = name,
                description = description,
                defaults = new ApiServiceDefaults
                {
                    duration = 45,
                    colour = "orange"
                },
                repetition = new ApiServiceRepetition
                {
                    repeatTimes = 1
                }
            };

            return JsonConvert.SerializeObject(service);
        }


        [TestFixture]
        public class ServiceCommandTests : ServiceTests
        {
            [Test]
            public void GivenNoServiceSaveCommand_WhenPost_ThenReturnNoDataError()
            {
                var command = GivenNoServiceSaveCommand();
                var response = WhenPost(command);
                AssertSingleError(response, "Please post us some data!");
            }

            [Test]
            public void GivenEmptyServiceSaveCommand_WhenPost_ThenReturnMultipleErrors()
            {
                var command = GivenEmptyServiceSaveCommand();
                var response = WhenPost(command);
                AssertMultipleErrors(response, new[,] { { "The businessId field is required.", "service.businessId" }, 
                                                        { "The name field is required.", "service.name" },
                                                        { "The repetition field is required.", "service.repetition" } });
            }

            [Test]
            public void GivenNonExistentBusinessId_WhenPost_ThenReturnInvalidBusinessIdError()
            {
                var command = GivenNonExistentBusinessId();
                var response = WhenPost(command);
                AssertSingleError(response, "This business does not exist.", "service.businessId");
            }


            private string GivenNoServiceSaveCommand()
            {
                return "";
            }

            private string GivenEmptyServiceSaveCommand()
            {
                return "{}";
            }

            private ApiServiceSaveCommand GivenNonExistentBusinessId()
            {
                return new ApiServiceSaveCommand
                {
                    businessId = Guid.Empty,
                    name = RandomString,
                    description = RandomString,
                    repetition = new ApiServiceRepetition { repeatTimes = 1 }
                };
            }
        }


        [TestFixture]
        public class ServiceNewTests : ServiceTests
        {
            [Test]
            public void GivenAnAlreadyExistingServiceName_WhenPost_ThenReturnDuplicateServiceError()
            {
                var command = GivenAnAlreadyExistingServiceName();
                var response = WhenPost(command);
                AssertSingleError(response, "This service already exists.", "service.name");
            }

            [Test]
            public void GivenNewUniqueService_WhenPost_ThenReturnNewServiceSuccess()
            {
                var command = GivenNewUniqueService();
                var response = WhenPost(command);
                AssertNewServiceSuccess(response);
            }

            [Test]
            public void GivenNewServiceWithInvalidBooking_WhenPost_ThenReturnInvalidBookingErrors()
            {
                var command = GivenNewServiceWithBooking(-8, true);
                var response = WhenPost(command);
                AssertSingleError(response, "The studentCapacity is not valid.", "service.booking.studentCapacity");
            }

            [Test]
            public void GivenNewServiceWithValidBooking_WhenPost_ThenReturnNewServiceSuccess()
            {
                var command = GivenNewServiceWithBooking(12, true);
                var response = WhenPost(command);
                AssertNewServiceWithBookingSuccess(response, 12, true);
            }




            [Test]
            public void GivenNewServiceWithInvalidDefaults_WhenPost_ThenReturnInvalidDefaultsErrors()
            {
                var command = GivenNewServiceWithInvalidDefaults();
                var response = WhenPost(command);
                AssertMultipleErrors(response, new[,] { { "The duration is not valid.", "service.defaults.duration" }, 
                                                        { "The colour is not valid.", "service.defaults.colour" } });
            }

            [Test]
            public void GivenNewServiceWithValidDefaults_WhenPost_ThenReturnNewServiceWitDefaultsSuccess()
            {
                var command = GivenNewServiceWithValidDefaults();
                var response = WhenPost(command);
                AssertNewServiceWithDefaultsSuccess(response);
            }

            [Test]
            public void GivenNewServiceWithEmptyPricingStructure_WhenPost_ThenReturnInvalidPricingError()
            {
                var command = GivenNewServiceWithPricing(null, null);
                var response = WhenPost(command);
                AssertSingleError(response, "This service is priced but has neither sessionPrice nor coursePrice.", "service.pricing");
            }

            [Test]
            public void GivenNewServiceWithSessionPriceOnly_WhenPost_ThenReturnNewServiceSuccess()
            {
                var command = GivenNewServiceWithPricing(15, null);
                var response = WhenPost(command);
                AssertNewServiceWithPricingSuccess(response, 15, null);
            }

            [Test]
            public void GivenSessionServiceWithCoursePrice_WhenPost_ThenReturnServiceDefaultsError()
            {
                var command = GivenSessionServiceWithCoursePrice();
                var response = WhenPost(command);
                AssertSingleError(response, 
                                  "The coursePrice cannot be specified if the service is not for a course or is open-ended.",
                                  "service.pricing.coursePrice");
            }

            [Test]
            public void GivenServiceWithoutRepetition_WhenPost_ThenReturnServiceRepetitionError()
            {
                var command = GivenServiceWithoutRepetition();
                var response = WhenPost(command);
                AssertSingleError(response, "The repetition field is required.", "service.repetition");
            }

            [Test]
            public void GivenOpenEndedCourseServiceWithCoursePrice_WhenPost_ThenReturnServiceDefaultsCoursePriceError()
            {
                var command = GivenOpenEndedCourseServiceWithCoursePrice();
                var response = WhenPost(command);
                AssertSingleError(response,
                                  "The coursePrice cannot be specified if the service is not for a course or is open-ended.",
                                  "service.pricing.coursePrice");
            }

            [Test]
            public void GivenFiniteCourseServiceWithoutCoursePrice_WhenPost_ThenCalculateTheCoursePrice()
            {
                var command = GivenFiniteCourseServiceWithoutCoursePrice();
                var response = WhenPost(command);
                AssertNewServiceWithPricingSuccess(response, 15, 150);
            }

            [Test]
            public void GivenUnpricedCourseService_WhenPost_ThenReturnNewServiceWithRepetitionSuccess()
            {
                var command = GivenUnpricedCourseService();
                var response = WhenPost(command);
                AssertNewServiceWithoutPricingSuccess(response);
            }

            [Test]
            public void GivenInvalidCourseService_WhenPost_ThenReturnServiceRepetitionErrors()
            {
                var command = GivenInvalidCourseService();
                var response = WhenPost(command);
                AssertMultipleErrors(response, new[,] { { "The repeatTimes field is not valid.", "service.repetition.repeatTimes" },
                                                        { "The repeatFrequency field is not valid.", "service.repetition.repeatFrequency" } });
            }

            [Test]
            public void GivenMultipleErrorsInService_WhenPost_ThenReturnServiceErrors()
            {
                var command = GivenMultipleErrorsInService();
                var response = WhenPost(command);
                AssertMultipleErrors(response, new[,] { { "The duration is not valid.", "service.defaults.duration" }, 
                                                        { "The colour is not valid.", "service.defaults.colour" },
                                                        { "The studentCapacity is not valid.", "service.booking.studentCapacity" },
                                                        { "This service is priced but has neither sessionPrice nor coursePrice.", "service.pricing" },
                                                        { "The repeatFrequency field is not valid.", "service.repetition.repeatFrequency" } });
            }


            private ApiServiceSaveCommand GivenNewSessionService()
            {
                return new ApiServiceSaveCommand
                {
                    businessId = BusinessId,
                    name = "Mini Orange",
                    description = "Mini Orange Service",
                    repetition = new ApiServiceRepetition { repeatTimes = 1 }
                };
            }

            private ApiServiceSaveCommand GivenNewUniqueService()
            {
                return GivenNewSessionService();
            }

            private ApiServiceSaveCommand GivenAnAlreadyExistingServiceName()
            {
                var service = GivenNewSessionService();
                service.name = MINI_RED_NAME;

                return service;
            }

            private ApiServiceSaveCommand GivenNewServiceWithValidDefaults()
            {
                var service = GivenNewSessionService();

                service.defaults = new ApiServiceDefaults
                {
                    duration = 60,
                    colour = " Orange"
                };

                return service;
            }

            private ApiServiceSaveCommand GivenNewServiceWithBooking(int? studentCapacity, bool? isOnlineBookable)
            {
                var service = GivenNewSessionService();

                service.booking = new ApiServiceBooking
                {
                    studentCapacity = studentCapacity,
                    isOnlineBookable = isOnlineBookable,
                };

                return service;
            }

            private ApiServiceSaveCommand GivenNewServiceWithInvalidDefaults()
            {
                var service = GivenNewSessionService();

                service.defaults = new ApiServiceDefaults
                {
                    duration = 67,
                    colour = "mandarin"
                };

                return service;
            }

            private ApiServiceSaveCommand GivenNewServiceWithPricing(decimal? sessionPrice, decimal? coursePrice)
            {
                var service = GivenNewSessionService();

                service.pricing = new ApiServicePricing
                {
                    sessionPrice = sessionPrice,
                    coursePrice = coursePrice
                };

                return service;
            }

            private ApiServiceSaveCommand GivenServiceWithoutRepetition()
            {
                var service = GivenNewSessionService();

                service.repetition = null;

                return service;
            }

            private ApiServiceSaveCommand GivenSessionServiceWithCoursePrice()
            {
                var service = GivenNewSessionService();

                service.pricing = new ApiServicePricing
                {
                    sessionPrice = 15,
                    coursePrice = 100
                };

                return service;
            }

            private ApiServiceSaveCommand GivenOpenEndedCourseServiceWithCoursePrice()
            {
                var service = GivenNewSessionService();

                service.repetition = new ApiServiceRepetition
                {
                    repeatFrequency = "d",
                    repeatTimes = -1 // Open-Ended
                };

                service.pricing = new ApiServicePricing
                {
                    sessionPrice = 15,
                    coursePrice = 100
                };

                return service;
            }

            private ApiServiceSaveCommand GivenFiniteCourseServiceWithoutCoursePrice()
            {
                var service = GivenNewSessionService();

                service.repetition = new ApiServiceRepetition
                {
                    repeatFrequency = "w",
                    repeatTimes = 10
                };

                service.pricing = new ApiServicePricing
                {
                    sessionPrice = 15,
                    coursePrice = null
                };

                return service;
            }

            private ApiServiceSaveCommand GivenUnpricedCourseService()
            {
                var service = GivenNewSessionService();

                service.repetition = new ApiServiceRepetition
                {
                    repeatFrequency = "w",
                    repeatTimes = 10
                };

                return service;
            }

            private ApiServiceSaveCommand GivenInvalidCourseService()
            {
                var service = GivenNewSessionService();

                service.repetition = new ApiServiceRepetition
                {
                    repeatFrequency = "xxx",
                    repeatTimes = -12
                };

                return service;
            }

            private ApiServiceSaveCommand GivenMultipleErrorsInService()
            {
                var service = GivenNewSessionService();

                service.defaults = new ApiServiceDefaults
                {
                    duration = 80,
                    colour = "Lime"
                };

                service.booking = new ApiServiceBooking
                {
                    studentCapacity = -8,
                    isOnlineBookable = true
                };

                service.repetition = new ApiServiceRepetition
                {
                    repeatFrequency = "fred",
                    repeatTimes = 12
                };

                service.pricing = new ApiServicePricing();

                return service;
            }


            private ServiceData AssertNewServiceSuccess(Response response)
            {
                Assert.That(response, Is.Not.Null);
                AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

                Assert.That(response.Payload, Is.InstanceOf<ServiceData>());
                var service = (ServiceData)response.Payload;

                Assert.That(service.id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(service.name, Is.EqualTo("Mini Orange"));
                Assert.That(service.description, Is.EqualTo("Mini Orange Service"));

                return service;
            }

            private void AssertNewServiceWithDefaultsSuccess(Response response)
            {
                var service = AssertNewServiceSuccess(response);
                AssertDefaults(service.defaults);
            }

            private void AssertNewServiceWithBookingSuccess(Response response, int? studentCapacity, bool? isOnlineBookable)
            {
                var service = AssertNewServiceSuccess(response);
                AssertBooking(service.booking, studentCapacity, isOnlineBookable);
            }

            private void AssertNewServiceWithPricingSuccess(Response response, decimal? sessionPrice, decimal? coursePrice)
            {
                var service = AssertNewServiceSuccess(response);
                AssertPricing(service.pricing, sessionPrice, coursePrice);
            }

            private void AssertNewServiceWithoutPricingSuccess(Response response)
            {
                var service = AssertNewServiceSuccess(response);
                Assert.That(service.pricing, Is.Null);
            }

            private void AssertDefaults(ServiceDefaults defaults)
            {
                Assert.That(defaults, Is.Not.Null);
                Assert.That(defaults.duration, Is.EqualTo(60));
                Assert.That(defaults.colour, Is.EqualTo("orange"));
            }

            private void AssertBooking(ServiceBooking booking, int? studentCapacity, bool? isOnlineBookable)
            {
                Assert.That(booking, Is.Not.Null);
                Assert.That(booking.studentCapacity, Is.EqualTo(studentCapacity));
                Assert.That(booking.isOnlineBookable, Is.EqualTo(isOnlineBookable));
            }

            private void AssertPricing(ServicePricing pricing, decimal? sessionPrice, decimal? coursePrice)
            {
                Assert.That(pricing, Is.Not.Null);
                Assert.That(pricing.sessionPrice, Is.EqualTo(sessionPrice));
                Assert.That(pricing.coursePrice, Is.EqualTo(coursePrice));
            }
        }


        [TestFixture]
        public class ServiceExistingTests : ServiceTests
        {
            [Test]
            public void GivenNonExistentServiceId_WhenPost_ThenReturnInvalidServiceIdError()
            {
                var command = GivenNonExistentServiceId();
                var response = WhenPost(command);
                AssertSingleError(response, "This service does not exist.", "service.id");
            }

            [Test]
            public void GivenChangeToAnAlreadyExistingServiceName_WhenPost_ThenReturnDuplicateServiceError()
            {
                var command = GivenChangeToAnAlreadyExistingServiceName();
                var response = WhenPost(command);
                AssertSingleError(response, "This service already exists.", "service.name");
            }

            [Test]
            public void GivenChangeToUniqueServiceName_WhenPost_ThenReturnExistingServiceSuccessResponse()
            {
                var command = GivenChangeToUniqueServiceName();
                var response = WhenPost(command);
                ThenReturnExistingServiceSuccessResponse(response);
            }

            [Test]
            public void GivenKeepServiceNameSame_WhenPost_ThenReturnExistingServiceSuccessResponse()
            {
                var command = GivenKeepServiceNameSame();
                var response = WhenPost(command);
                ThenReturnExistingServiceSuccessResponse(response);
            }

            [Test]
            public void GivenExistingServiceWithDefaults_WhenPost_ThenReturnExistingServiceWithDefaultsSuccessResponse()
            {
                var command = GivenExistingServiceWithDefaults();
                var response = WhenPost(command);
                ThenReturnExistingServiceWithDefaultsSuccessResponse(response);
            }

            [Test]
            public void GivenExistingServiceWithRepetition_WhenPost_ThenReturnExistingServiceWithRepetitionSuccessResponse()
            {
                var command = GivenExistingServiceWithRepetition();
                var response = WhenPost(command);
                ThenReturnExistingServiceWithRepetitionSuccessResponse(response);
            }

            [Test]
            public void GivenExistingServiceWithPricing_WhenPost_ThenReturnExistingServiceWithPricingSuccessResponse()
            {
                var command = GivenExistingServiceWithPricing();
                var response = WhenPost(command);
                ThenReturnExistingServiceWithPricingSuccessResponse(response);
            }


            private ApiServiceSaveCommand GivenExistingSessionService()
            {
                return new ApiServiceSaveCommand
                {
                    businessId = BusinessId,
                    id = MiniRedId,
                    name = MINI_RED_NAME,
                    description = "Mini Red Service",
                    repetition = new ApiServiceRepetition { repeatTimes = 1 }
                };
            }

            private ApiServiceSaveCommand GivenNonExistentServiceId()
            {
                var service = GivenExistingSessionService();
                service.id = Guid.NewGuid();

                return service;
            }

            private ApiServiceSaveCommand GivenChangeToAnAlreadyExistingServiceName()
            {
                var service = GivenExistingSessionService();
                service.name = MINI_BLUE_NAME;

                return service;
            }

            private ApiServiceSaveCommand GivenChangeToUniqueServiceName()
            {
                var service = GivenExistingSessionService();
                service.name = NewServiceName = "Mini Red #3";
                service.description = NewServiceDescription = "Mini Red #3 Service";

                return service;
            }

            private ApiServiceSaveCommand GivenKeepServiceNameSame()
            {
                var service = GivenExistingSessionService();
                service.name = NewServiceName = MINI_RED_NAME;
                service.description = NewServiceDescription = "Mini Red Service";

                return service;
            }

            private ApiServiceSaveCommand GivenExistingServiceWithDefaults()
            {
                var service = GivenExistingSessionService();
                service.defaults = new ApiServiceDefaults
                {
                    duration = Duration = 60,
                    colour = Colour = "Red"
                };

                return service;
            }

            private ApiServiceSaveCommand GivenExistingServiceWithRepetition()
            {
                var service = GivenExistingSessionService();
                service.repetition = new ApiServiceRepetition
                {
                    repeatFrequency = "2d",
                    repeatTimes = 15
                };

                return service;
            }

            private ApiServiceSaveCommand GivenExistingServiceWithPricing()
            {
                var service = GivenExistingSessionService();
                service.pricing = new ApiServicePricing
                {
                    sessionPrice = 16.99m,
                    coursePrice = 149.99m,
                };
                service.repetition = new ApiServiceRepetition
                {
                    repeatFrequency = "w",
                    repeatTimes = 10
                };

                return service;
            }


            private ServiceData AssertSuccess(Response response)
            {
                Assert.That(response, Is.Not.Null);
                AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

                Assert.That(response.Payload, Is.InstanceOf<ServiceData>());
                var service = (ServiceData)response.Payload;

                return service;
            }

            private void ThenReturnExistingServiceSuccessResponse(Response response)
            {
                var service = AssertSuccess(response);

                Assert.That(service.id, Is.EqualTo(MiniRedId));
                Assert.That(service.name, Is.EqualTo(NewServiceName));
                Assert.That(service.description, Is.EqualTo(NewServiceDescription));
            }

            private void ThenReturnExistingServiceWithDefaultsSuccessResponse(Response response)
            {
                var service = AssertSuccess(response);

                Assert.That(service.id, Is.EqualTo(MiniRedId));
                Assert.That(service.name, Is.EqualTo(MINI_RED_NAME));
                Assert.That(service.description, Is.EqualTo(MINI_RED_DESCRIPTION));

                var defaults = service.defaults;
                Assert.That(defaults.duration, Is.EqualTo(Duration));
                Assert.That(defaults.colour, Is.EqualTo(Colour.ToLower()));
            }

            private void ThenReturnExistingServiceWithRepetitionSuccessResponse(Response response)
            {
                var service = AssertSuccess(response);

                Assert.That(service.id, Is.EqualTo(MiniRedId));
                Assert.That(service.name, Is.EqualTo(MINI_RED_NAME));
                Assert.That(service.description, Is.EqualTo(MINI_RED_DESCRIPTION));

                var repetition = service.repetition;
                Assert.That(repetition.repeatFrequency, Is.EqualTo("2d"));
                Assert.That(repetition.repeatTimes, Is.EqualTo(15));
            }

            private void ThenReturnExistingServiceWithPricingSuccessResponse(Response response)
            {
                var service = AssertSuccess(response);

                Assert.That(service.id, Is.EqualTo(MiniRedId));
                Assert.That(service.name, Is.EqualTo(MINI_RED_NAME));
                Assert.That(service.description, Is.EqualTo(MINI_RED_DESCRIPTION));

                var pricing = service.pricing;
                Assert.That(pricing.sessionPrice, Is.EqualTo(16.99m));
                Assert.That(pricing.coursePrice, Is.EqualTo(149.99m));

                var repetition = service.repetition;
                Assert.That(repetition.repeatFrequency, Is.EqualTo("w"));
                Assert.That(repetition.repeatTimes, Is.EqualTo(10));
            }
        }


        private Response WhenPost(string json)
        {
            return Post<ServiceData>(json);
        }

        private Response WhenPost(ApiServiceSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenPost(json);
        }
    }
}
