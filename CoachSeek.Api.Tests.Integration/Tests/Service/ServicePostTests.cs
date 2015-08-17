using System;
using System.Net;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Service
{
    public abstract class ServicePostTests : ServiceTests
    {
        [TestFixture]
        public class ServiceCommandTests : ServicePostTests
        {
            [Test]
            public void GivenNoServiceSaveCommand_WhenTryPost_ThenReturnNoDataError()
            {
                var setup = RegisterBusiness();

                var command = GivenNoServiceSaveCommand();
                var response = WhenTryPost(command, setup);
                AssertSingleError(response, "Please post us some data!");
            }

            [Test]
            public void GivenEmptyServiceSaveCommand_WhenTryPost_ThenReturnMultipleErrors()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyServiceSaveCommand();
                var response = WhenTryPost(command, setup);
                AssertMultipleErrors(response, new[,] { { "name-required", "The Name field is required.", null, null },
                                                        { "repetition-required", "The Repetition field is required.", null, null } });
            }

            [Test]
            public void GivenValidServiceSaveCommand_WhenTryPostAnonymously_ThenReturnUnauthorised()
            {
                var setup = RegisterBusiness();

                var command = GivenValidServiceSaveCommand();
                var response = WhenTryPostAnonymously(command, setup);
                AssertUnauthorised(response);
            }


            private string GivenNoServiceSaveCommand()
            {
                return "";
            }

            private string GivenEmptyServiceSaveCommand()
            {
                return "{}";
            }

            private string GivenValidServiceSaveCommand()
            {
                var command = new ApiServiceSaveCommand
                {
                    name = "Mini Orange",
                    description = "Mini Orange Service",
                    repetition = new ApiServiceRepetition { sessionCount = 1 },
                    presentation = new ApiPresentation { colour = "orange" }
                };

                return JsonSerialiser.Serialise(command);
            }
        }


        [TestFixture]
        public class ServiceNewTests : ServicePostTests
        {
            [Test]
            public void GivenAnAlreadyExistingServiceName_WhenTryPost_ThenReturnDuplicateServiceError()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var command = GivenAnAlreadyExistingServiceName(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnDuplicateServiceError(response);
            }

            [Test]
            public void GivenNewUniqueService_WhenTryPost_ThenCreateNewService()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var command = GivenNewUniqueService();
                var response = WhenTryPost(command, setup);
                AssertNewServiceSuccess(response);
            }

            [Test]
            public void GivenNewServiceWithInvalidBooking_WhenTryPost_ThenReturnInvalidBookingErrors()
            {
                var setup = RegisterBusiness();

                var command = GivenNewServiceWithBooking(-8, true);
                var response = WhenTryPost(command, setup);
                AssertSingleError(response, "The studentCapacity field is not valid.", "service.booking.studentCapacity");
            }

            [Test]
            public void GivenNewServiceWithValidBooking_WhenTryPost_ThenCreateNewService()
            {
                var setup = RegisterBusiness();

                var command = GivenNewServiceWithBooking(12, true);
                var response = WhenTryPost(command, setup);
                AssertNewServiceWithBookingSuccess(response, 12, true);
            }

            [Test]
            public void GivenNewServiceWithInvalidDefaults_WhenTryPost_ThenReturnInvalidDefaultsErrors()
            {
                var setup = RegisterBusiness();

                var command = GivenNewServiceWithInvalidDefaults();
                var response = WhenTryPost(command, setup);
                AssertMultipleErrors(response, new[,] { { null, "The duration field is not valid.", null, "service.timing.duration" }, 
                                                        { ErrorCodes.ColourInvalid, "Colour 'mandarin' is not valid.", "mandarin", null } });
            }

            [Test]
            public void GivenNewServiceWithValidDefaults_WhenTryPost_ThenReturnNewServiceWitDefaultsSuccess()
            {
                var setup = RegisterBusiness();

                var command = GivenNewServiceWithValidDefaults();
                var response = WhenTryPost(command, setup);
                AssertNewServiceWithDefaultsSuccess(response);
            }

            [Test]
            public void GivenNewServiceWithEmptyPricingStructure_WhenTryPost_ThenReturnInvalidPricingError()
            {
                var setup = RegisterBusiness();

                var command = GivenNewServiceWithPricing(null, null);
                var response = WhenTryPost(command, setup);
                AssertSingleError(response, "This service is priced but has neither sessionPrice nor coursePrice.", "service.pricing");
            }

            [Test]
            public void GivenNewServiceWithSessionPriceOnly_WhenTryPost_ThenReturnNewServiceSuccess()
            {
                var setup = RegisterBusiness();

                var command = GivenNewServiceWithPricing(15, null);
                var response = WhenTryPost(command, setup);
                AssertNewServiceWithPricingSuccess(response, 15, null);
            }

            [Test]
            public void GivenSessionServiceWithCoursePrice_WhenTryPost_ThenReturnServiceDefaultsError()
            {
                var setup = RegisterBusiness();

                var command = GivenSessionServiceWithCoursePrice();
                var response = WhenTryPost(command, setup);
                AssertSingleError(response, 
                                  "The coursePrice cannot be specified if the service is not for a course or is open-ended.",
                                  "service.pricing.coursePrice");
            }

            [Test]
            public void GivenServiceWithoutRepetition_WhenTryPost_ThenReturnServiceRepetitionError()
            {
                var setup = RegisterBusiness();

                var command = GivenServiceWithoutRepetition();
                var response = WhenTryPost(command, setup);
                AssertSingleError(response, "repetition-required", "The Repetition field is required.", null);
            }

            [Test]
            public void GivenServiceWithoutColour_WhenTryCreateService_ThenCreateServiceAndDefaultColourToGreen()
            {
                var setup = RegisterBusiness();

                var command = GivenServiceWithoutColour();
                var response = WhenTryCreateService(command, setup);
                ThenCreateServiceAndDefaultColourToGreen(response);
            }

            [Test]
            public void GivenOpenEndedCourseServiceWithCoursePrice_WhenTryPost_ThenReturnServiceDefaultsCoursePriceError()
            {
                var setup = RegisterBusiness();

                var command = GivenOpenEndedCourseServiceWithCoursePrice();
                var response = WhenTryPost(command, setup);
                AssertSingleError(response,
                                  "The coursePrice cannot be specified if the service is not for a course or is open-ended.",
                                  "service.pricing.coursePrice");
            }

            [Test]
            public void GivenFiniteCourseServiceWithoutCoursePrice_WhenTryPost_ThenCalculateTheCoursePrice()
            {
                var setup = RegisterBusiness();

                var command = GivenFiniteCourseServiceWithoutCoursePrice();
                var response = WhenTryPost(command, setup);
                AssertNewServiceWithPricingSuccess(response, 15, 150);
            }

            [Test]
            public void GivenUnpricedCourseService_WhenTryPost_ThenReturnNewServiceWithRepetitionSuccess()
            {
                var setup = RegisterBusiness();

                var command = GivenUnpricedCourseService();
                var response = WhenTryPost(command, setup);
                AssertNewServiceWithoutPricingSuccess(response);
            }

            [Test]
            public void GivenInvalidCourseService_WhenTryPost_ThenReturnServiceRepetitionErrors()
            {
                var setup = RegisterBusiness();

                var command = GivenInvalidCourseService();
                var response = WhenTryPost(command, setup);
                AssertMultipleErrors(response, new[,] { { null, "The sessionCount field is not valid.", null, "service.repetition.sessionCount" },
                                                        { null, "The repeatFrequency field is not valid.", null, "service.repetition.repeatFrequency" } });
            }

            [Test]
            public void GivenMultipleErrorsInService_WhenTryPost_ThenReturnServiceErrors()
            {
                var setup = RegisterBusiness();

                var command = GivenMultipleErrorsInService();
                var response = WhenTryCreateService(command, setup);
                AssertMultipleErrors(response, new[,] { { null, "The duration field is not valid.", null, "service.timing.duration" }, 
                                                        { null, "The studentCapacity field is not valid.", null, "service.booking.studentCapacity" },
                                                        { null, "This service is priced but has neither sessionPrice nor coursePrice.", null, "service.pricing" },
                                                        { null, "The repeatFrequency field is not valid.", null, "service.repetition.repeatFrequency" },
                                                        { ErrorCodes.ColourInvalid, "Colour 'lime' is not valid.", "lime", null } });
            }

            [Test]
            public void GivenSessionServiceWithAllDefaultValuesSpecified_WhenTryPost_ThenReturnNewSessionServiceWithAllDefaultValuesSuccess()
            {
                var setup = RegisterBusiness();

                var command = GivenSessionServiceWithAllDefaultValuesSpecified();
                var response = WhenTryPost(command, setup);
                AssertNewSessionServiceWithAllDefaultValuesSuccess(response);
            }

            [Test]
            public void GivenCourseServiceWithAllDefaultValuesSpecified_WhenTryPost_ThenReturnNewCourseServiceWithAllDefaultValuesSuccess()
            {
                var setup = RegisterBusiness();

                var command = GivenCourseServiceWithAllDefaultValuesSpecified();
                var response = WhenTryPost(command, setup);
                AssertNewCourseServiceWithAllDefaultValuesSuccess(response);
            }

            [Test]
            public void GivenWantToUpdateExistingService_WhenTryPost_ThenReturnUpdatedService()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var command = GivenWantToUpdateExistingService(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnUpdatedService(response, setup);
            }


            private string GivenWantToUpdateExistingService(SetupData setup)
            {
                var command = new ApiServiceSaveCommand
                {
                    id = setup.MiniRed.Id,
                    name = "Mini Yellow",
                    description = "Mini Yellow Service",
                    timing = new ApiServiceTiming { duration = 60 },
                    booking = new ApiServiceBooking { studentCapacity = 12, isOnlineBookable = true },
                    presentation = new ApiPresentation { colour = "Yellow" },
                    repetition = new ApiServiceRepetition { sessionCount = 9, repeatFrequency = "d" },
                    pricing = new ApiPricing {sessionPrice = 10, coursePrice = 80}
                };

                return JsonSerialiser.Serialise(command);
            }


            private void ThenReturnDuplicateServiceError(ApiResponse response)
            {
                AssertSingleError(response, ErrorCodes.ServiceDuplicate, "Service 'Mini Red' already exists.", "Mini Red");
            }

            private void ThenCreateServiceAndDefaultColourToGreen(ApiResponse response)
            {
                var service = AssertSuccessResponse<ServiceData>(response);
                Assert.That(service.presentation.colour, Is.EqualTo("green"));
            }

            private void ThenReturnUpdatedService(ApiResponse response, SetupData setup)
            {
                Assert.That(response, Is.Not.Null);
                AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

                Assert.That(response.Payload, Is.InstanceOf<ServiceData>());
                var service = (ServiceData)response.Payload;

                Assert.That(service.id, Is.EqualTo(setup.MiniRed.Id));
                Assert.That(service.name, Is.EqualTo("Mini Yellow"));
                Assert.That(service.description, Is.EqualTo("Mini Yellow Service"));
                Assert.That(service.timing.duration, Is.EqualTo(60));
                Assert.That(service.booking.studentCapacity, Is.EqualTo(12));
                Assert.That(service.booking.isOnlineBookable, Is.EqualTo(true));
                Assert.That(service.presentation.colour, Is.EqualTo("yellow"));
                Assert.That(service.repetition.sessionCount, Is.EqualTo(9));
                Assert.That(service.repetition.repeatFrequency, Is.EqualTo("d"));
                Assert.That(service.pricing.sessionPrice, Is.EqualTo(10));
                Assert.That(service.pricing.coursePrice, Is.EqualTo(80));
            }

            private ApiServiceSaveCommand GivenNewSessionService()
            {
                return new ApiServiceSaveCommand
                {
                    name = "Mini Orange",
                    description = "Mini Orange Service",
                    repetition = new ApiServiceRepetition { sessionCount = 1 },
                    presentation = new ApiPresentation { colour = "orange" }
                };
            }

            private ApiServiceSaveCommand GivenNewUniqueService()
            {
                return GivenNewSessionService();
            }

            private ApiServiceSaveCommand GivenAnAlreadyExistingServiceName(SetupData setup)
            {
                var service = GivenNewSessionService();
                service.name = setup.MiniRed.Name;

                return service;
            }

            private ApiServiceSaveCommand GivenNewServiceWithValidDefaults()
            {
                var service = GivenNewSessionService();

                service.timing = new ApiServiceTiming { duration = 60 };
                service.presentation = new ApiPresentation { colour = " Orange" };

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

                service.timing = new ApiServiceTiming { duration = 67 };
                service.presentation = new ApiPresentation { colour = "mandarin" };

                return service;
            }

            private ApiServiceSaveCommand GivenNewServiceWithPricing(decimal? sessionPrice, decimal? coursePrice)
            {
                var service = GivenNewSessionService();

                service.pricing = new ApiPricing
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

            private ApiServiceSaveCommand GivenServiceWithoutColour()
            {
                var service = GivenNewSessionService();

                service.presentation = null;

                return service;
            }

            private ApiServiceSaveCommand GivenSessionServiceWithCoursePrice()
            {
                var service = GivenNewSessionService();

                service.pricing = new ApiPricing
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
                    sessionCount = -1,  // Open-Ended
                    repeatFrequency = "d"
                };

                service.pricing = new ApiPricing
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
                    sessionCount = 10,
                    repeatFrequency = "w"
                };

                service.pricing = new ApiPricing
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
                    sessionCount = 10,
                    repeatFrequency = "w"
                };

                return service;
            }

            private ApiServiceSaveCommand GivenInvalidCourseService()
            {
                var service = GivenNewSessionService();

                service.repetition = new ApiServiceRepetition
                {
                    sessionCount = -12,
                    repeatFrequency = "xxx"
                };

                return service;
            }

            private ApiServiceSaveCommand GivenMultipleErrorsInService()
            {
                var service = GivenNewSessionService();

                service.timing = new ApiServiceTiming { duration = 80 };

                service.booking = new ApiServiceBooking
                {
                    studentCapacity = -8,
                    isOnlineBookable = true
                };

                service.repetition = new ApiServiceRepetition
                {
                    sessionCount = 12,
                    repeatFrequency = "fred"
                };

                service.pricing = new ApiPricing();

                service.presentation = new ApiPresentation { colour = "Lime" };

                return service;
            }

            private ApiServiceSaveCommand GivenSessionServiceWithAllDefaultValuesSpecified()
            {
                var miniGreenService = GivenBaseMiniGreenService();

                miniGreenService.repetition = new ApiServiceRepetition { sessionCount = 1 };
                miniGreenService.pricing = new ApiPricing { sessionPrice = 15 };

                return miniGreenService;
            }

            private ApiServiceSaveCommand GivenCourseServiceWithAllDefaultValuesSpecified()
            {
                var miniGreenService = GivenBaseMiniGreenService();

                miniGreenService.repetition = new ApiServiceRepetition { sessionCount = 12, repeatFrequency = "w" };
                miniGreenService.pricing = new ApiPricing {sessionPrice = 15, coursePrice = 150};

                return miniGreenService;
            }

            private ApiServiceSaveCommand GivenBaseMiniGreenService()
            {
                return new ApiServiceSaveCommand
                {
                    name = "Mini Green",
                    description = "Mini Green Service",
                    timing = new ApiServiceTiming { duration = 45 },
                    booking = new ApiServiceBooking { studentCapacity = 8, isOnlineBookable = false },
                    presentation = new ApiPresentation { colour = "Green" }
                };
            }


            private ServiceData AssertNewServiceSuccess(ApiResponse response)
            {
                var service = AssertSuccessResponse<ServiceData>(response);

                Assert.That(service.id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(service.name, Is.EqualTo("Mini Orange"));
                Assert.That(service.description, Is.EqualTo("Mini Orange Service"));

                return service;
            }

            private void AssertNewServiceWithDefaultsSuccess(ApiResponse response)
            {
                var service = AssertNewServiceSuccess(response);
                AssertDefaults(service);
            }

            private void AssertNewServiceWithBookingSuccess(ApiResponse response, int? studentCapacity, bool? isOnlineBookable)
            {
                var service = AssertNewServiceSuccess(response);
                AssertBooking(service.booking, studentCapacity, isOnlineBookable);
            }

            private void AssertNewServiceWithPricingSuccess(ApiResponse response, decimal? sessionPrice, decimal? coursePrice)
            {
                var service = AssertNewServiceSuccess(response);
                AssertPricing(service.pricing, sessionPrice, coursePrice);
            }

            private void AssertNewServiceWithoutPricingSuccess(ApiResponse response)
            {
                var service = AssertNewServiceSuccess(response);
                Assert.That(service.pricing, Is.Null);
            }

            private void AssertNewSessionServiceWithAllDefaultValuesSuccess(ApiResponse response)
            {
                var miniGreenService = AssertMiniGreenServiceSuccess(response);

                var repetition = miniGreenService.repetition;
                Assert.That(repetition, Is.Not.Null);
                Assert.That(repetition.sessionCount, Is.EqualTo(1));
                Assert.That(repetition.repeatFrequency, Is.Null);

                var pricing = miniGreenService.pricing;
                Assert.That(pricing, Is.Not.Null);
                Assert.That(pricing.sessionPrice, Is.EqualTo(15));
                Assert.That(pricing.coursePrice, Is.Null);
            }

            private void AssertNewCourseServiceWithAllDefaultValuesSuccess(ApiResponse response)
            {
                var miniGreenService = AssertMiniGreenServiceSuccess(response);

                var repetition = miniGreenService.repetition;
                Assert.That(repetition, Is.Not.Null);
                Assert.That(repetition.sessionCount, Is.EqualTo(12));
                Assert.That(repetition.repeatFrequency, Is.EqualTo("w"));

                var pricing = miniGreenService.pricing;
                Assert.That(pricing, Is.Not.Null);
                Assert.That(pricing.sessionPrice, Is.EqualTo(15));
                Assert.That(pricing.coursePrice, Is.EqualTo(150));
            }

            private ServiceData AssertMiniGreenServiceSuccess(ApiResponse response)
            {
                var service = AssertSuccessResponse<ServiceData>(response);

                Assert.That(service.id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(service.name, Is.EqualTo("Mini Green"));
                Assert.That(service.description, Is.EqualTo("Mini Green Service"));

                var timing = service.timing;
                Assert.That(timing, Is.Not.Null);
                Assert.That(timing.duration, Is.EqualTo(45));

                var booking = service.booking;
                Assert.That(booking, Is.Not.Null);
                Assert.That(booking.studentCapacity, Is.EqualTo(8));
                Assert.That(booking.isOnlineBookable, Is.EqualTo(false));

                var presentation = service.presentation;
                Assert.That(presentation, Is.Not.Null);
                Assert.That(presentation.colour, Is.EqualTo("green"));

                return service;
            }

            private void AssertDefaults(ServiceData service)
            {
                Assert.That(service.timing, Is.Not.Null);
                Assert.That(service.timing.duration, Is.EqualTo(60));

                Assert.That(service.presentation, Is.Not.Null);
                Assert.That(service.presentation.colour, Is.EqualTo("orange"));
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
        public class ServiceExistingTests : ServicePostTests
        {
            [Test]
            public void GivenNonExistentServiceId_WhenPost_ThenReturnInvalidServiceIdError()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var command = GivenNonExistentServiceId(setup);
                var response = WhenTryPost(command, setup);
                AssertSingleError(response, ErrorCodes.ServiceInvalid, "This service does not exist.", command.id.Value.ToString());
            }

            [Test]
            public void GivenChangeToAnAlreadyExistingServiceName_WhenPost_ThenReturnDuplicateServiceError()
            {
                var setup = RegisterBusiness();
                RegisterTestServices(setup);

                var command = GivenChangeToAnAlreadyExistingServiceName(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnDuplicateServiceError(response);
            }

            [Test]
            public void GivenChangeToUniqueServiceName_WhenTryPost_ThenChangeServiceName()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var command = GivenChangeToUniqueServiceName(setup);
                var response = WhenTryPost(command, setup);
                ThenChangeServiceName(response, setup);
            }

            [Test]
            public void GivenKeepServiceNameSame_WhenTryPost_ThenDoNotChangeServiceName()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);
                
                var command = GivenKeepServiceNameSame(setup);
                var response = WhenTryPost(command, setup);
                ThenDoNotChangeServiceName(response, setup);
            }

            [Test]
            public void GivenExistingServiceWithDefaults_WhenTryPost_ThenReturnExistingServiceWithDefaultsSuccessResponse()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var command = GivenExistingServiceWithDefaults(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnExistingServiceWithDefaultsSuccess(response, setup);
            }

            [Test]
            public void GivenExistingServiceWithRepetition_WhenPost_ThenReturnExistingServiceWithRepetitionSuccessResponse()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var command = GivenExistingServiceWithRepetition(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnExistingServiceWithRepetitionSuccess(response, setup);
            }

            [Test]
            public void GivenExistingServiceWithPricing_WhenPost_ThenReturnExistingServiceWithPricingSuccessResponse()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var command = GivenExistingServiceWithPricing(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnExistingServiceWithPricingSuccess(response, setup);
            }


            private ApiServiceSaveCommand GivenExistingSessionService(SetupData setup)
            {
                return new ApiServiceSaveCommand(setup.MiniRed);

            }

            private ApiServiceSaveCommand GivenNonExistentServiceId(SetupData setup)
            {
                var service = GivenExistingSessionService(setup);
                service.id = Guid.NewGuid();

                return service;
            }

            private ApiServiceSaveCommand GivenChangeToAnAlreadyExistingServiceName(SetupData setup)
            {
                var service = GivenExistingSessionService(setup);
                service.name = setup.HolidayCamp.Name;

                return service;
            }

            private ApiServiceSaveCommand GivenChangeToUniqueServiceName(SetupData setup)
            {
                var service = GivenExistingSessionService(setup);
                service.name = "Mini Red #3";
                service.description = "Mini Red #3 Service";

                return service;
            }

            private ApiServiceSaveCommand GivenKeepServiceNameSame(SetupData setup)
            {
                var service = GivenExistingSessionService(setup);
                service.name = setup.MiniRed.Name;
                service.description = setup.MiniRed.Description;

                return service;
            }

            private ApiServiceSaveCommand GivenExistingServiceWithDefaults(SetupData setup)
            {
                var service = GivenExistingSessionService(setup);

                service.timing = new ApiServiceTiming { duration = 60 };
                service.presentation = new ApiPresentation { colour = "Red" };

                return service;
            }

            private ApiServiceSaveCommand GivenExistingServiceWithRepetition(SetupData setup)
            {
                var service = GivenExistingSessionService(setup);
                service.repetition = new ApiServiceRepetition
                {
                    sessionCount = 15,
                    repeatFrequency = "d"
                };

                return service;
            }

            private ApiServiceSaveCommand GivenExistingServiceWithPricing(SetupData setup)
            {
                var service = GivenExistingSessionService(setup);
                service.pricing = new ApiPricing
                {
                    sessionPrice = 16.99m,
                    coursePrice = 149.99m,
                };
                service.repetition = new ApiServiceRepetition
                {
                    sessionCount = 10,
                    repeatFrequency = "w"
                };

                return service;
            }


            private void ThenReturnDuplicateServiceError(ApiResponse response)
            {
                AssertSingleError(response, ErrorCodes.ServiceDuplicate, "Service 'Holiday Camp' already exists.", "Holiday Camp");
            }

            private void ThenDoNotChangeServiceName(ApiResponse response, SetupData setup)
            {
                var service = AssertSuccessResponse<ServiceData>(response);

                Assert.That(service.id, Is.EqualTo(setup.MiniRed.Id));
                Assert.That(service.name, Is.EqualTo(setup.MiniRed.Name));
                Assert.That(service.description, Is.EqualTo(setup.MiniRed.Description));
            }

            private void ThenChangeServiceName(ApiResponse response, SetupData setup)
            {
                var service = AssertSuccessResponse<ServiceData>(response);

                Assert.That(service.id, Is.EqualTo(setup.MiniRed.Id));
                Assert.That(service.name, Is.EqualTo("Mini Red #3"));
                Assert.That(service.description, Is.EqualTo("Mini Red #3 Service"));
            }

            private void ThenReturnExistingServiceWithDefaultsSuccess(ApiResponse response, SetupData setup)
            {
                var service = AssertSuccessResponse<ServiceData>(response);

                Assert.That(service.id, Is.EqualTo(setup.MiniRed.Id));
                Assert.That(service.name, Is.EqualTo(setup.MiniRed.Name));
                Assert.That(service.description, Is.EqualTo(setup.MiniRed.Description));

                var timing = service.timing;
                Assert.That(timing, Is.Not.Null);
                Assert.That(timing.duration, Is.EqualTo(60));

                var presentation = service.presentation;
                Assert.That(presentation, Is.Not.Null);
                Assert.That(presentation.colour, Is.EqualTo("red"));
            }

            private void ThenReturnExistingServiceWithRepetitionSuccess(ApiResponse response, SetupData setup)
            {
                var service = AssertSuccessResponse<ServiceData>(response);

                Assert.That(service.id, Is.EqualTo(setup.MiniRed.Id));
                Assert.That(service.name, Is.EqualTo(setup.MiniRed.Name));
                Assert.That(service.description, Is.EqualTo(setup.MiniRed.Description));

                var repetition = service.repetition;
                Assert.That(repetition.sessionCount, Is.EqualTo(15));
                Assert.That(repetition.repeatFrequency, Is.EqualTo("d"));
            }

            private void ThenReturnExistingServiceWithPricingSuccess(ApiResponse response, SetupData setup)
            {
                var service = AssertSuccessResponse<ServiceData>(response);

                Assert.That(service.id, Is.EqualTo(setup.MiniRed.Id));
                Assert.That(service.name, Is.EqualTo(setup.MiniRed.Name));
                Assert.That(service.description, Is.EqualTo(setup.MiniRed.Description));

                var pricing = service.pricing;
                Assert.That(pricing.sessionPrice, Is.EqualTo(16.99m));
                Assert.That(pricing.coursePrice, Is.EqualTo(149.99m));

                var repetition = service.repetition;
                Assert.That(repetition.sessionCount, Is.EqualTo(10));
                Assert.That(repetition.repeatFrequency, Is.EqualTo("w"));
            }
        }


        private ApiResponse WhenTryCreateService(ApiServiceSaveCommand command, SetupData setup)
        {
            return WhenTryPost(command, setup);
        }

        private ApiResponse WhenTryPost(ApiServiceSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryPost(json, setup);
        }

        private ApiResponse WhenTryPost(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<ServiceData>(json,
                                                                      setup.Business.UserName,
                                                                      setup.Business.Password,
                                                                      RelativePath);
        }

        private ApiResponse WhenTryPostAnonymously(string json, SetupData setup)
        {
            return new TestBusinessAnonymousApiClient().Post<ServiceData>(json,
                                                                          setup.Business.Domain,
                                                                          RelativePath);
        }
    }
}
