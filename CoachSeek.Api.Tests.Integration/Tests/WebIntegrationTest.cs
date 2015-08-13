using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using NUnit.Framework;
using System;
using System.Net;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    public abstract class WebIntegrationTest
    {
        protected abstract string RelativePath { get; }


        protected SetupData RegisterBusiness()
        {
            var business = new RandomBusiness();
            var response = BusinessRegistrar.RegisterBusiness(business);
            return new SetupData((RegistrationData)response.Payload, business.Password);
        }


        protected ApiResponse PostBooking(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<BookingData>(json,
                                                                      setup.Business.UserName,
                                                                      setup.Business.Password,
                                                                      "Bookings");
        }

        protected ApiResponse PostCourseBooking(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<CourseBookingData>(json,
                                                                            setup.Business.UserName,
                                                                            setup.Business.Password,
                                                                            "Bookings");
        }

        protected ApiResponse AdminGet<TResponse>(string relativePath)
        {
            return new TestAdminApiClient().Get<TResponse>(relativePath);
        }

        protected ApiResponse AuthenticatedGet<TResponse>(string relativePath, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Get<TResponse>(setup.Business.UserName,
                                                                   setup.Business.Password,
                                                                   relativePath);
        }

        protected ApiResponse AuthenticatedGet<TResponse>(string relativePath, Guid id, string username, string password)
        {
            var url = string.Format("{0}/{1}", relativePath, id);
            return new TestAuthenticatedApiClient().Get<TResponse>(username,
                                                                   password,
                                                                   url);
        }

        protected ApiResponse AuthenticatedGet<TResponse>(string relativePath, string username, string password)
        {
            return new TestAuthenticatedApiClient().Get<TResponse>(username,
                                                                   password,
                                                                   relativePath);
        }

        protected ApiResponse BusinessAnonymousGet<TResponse>(string relativePath, Guid id, string businessDomain)
        {
            var url = string.Format("{0}/{1}", relativePath, id);
            return BusinessAnonymousGet<TResponse>(url, businessDomain);
        }

        protected ApiResponse BusinessAnonymousGet<TResponse>(string relativePath, string businessDomain)
        {
            return new TestBusinessAnonymousApiClient().Get<TResponse>(businessDomain, relativePath);
        }

        protected ApiResponse AuthenticatedGet<TResponse>(string relativePath, Guid id, SetupData setup)
        {
            var url = string.Format("{0}/{1}", relativePath, id);
            return new TestAuthenticatedApiClient().Get<TResponse>(setup.Business.UserName,
                                                                   setup.Business.Password,
                                                                   url);
        }


        protected ApiResponse AuthenticatedPost<TResponse>(string json, string relativePath, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<TResponse>(json,
                                                                    setup.Business.UserName,
                                                                    setup.Business.Password,
                                                                    relativePath);
        }

        protected ApiResponse AnonymousPost<TResponse>(string json, string relativePath)
        {
            return new TestAnonymousApiClient().Post<TResponse>(json, RelativePath);
        }

        protected ApiResponse BusinessAnonymousPost<TResponse>(string json, string relativePath, SetupData setup)
        {
            return new TestBusinessAnonymousApiClient().Post<TResponse>(json,
                                                                        setup.Business.Domain,
                                                                        relativePath);
        }


        protected ApiResponse Delete(string relativePath, string id, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Delete(setup.Business.UserName,
                                                           setup.Business.Password,
                                                           relativePath,
                                                           id);
        }

        protected ApiResponse DeleteAnonymously(string relativePath, string id)
        {
            return new TestAnonymousApiClient().Delete(relativePath, id);
        }

        protected ApiResponse PostSession(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<SessionData>(json,
                                                                      setup.Business.UserName,
                                                                      setup.Business.Password,
                                                                      "Sessions");
        }

        protected ApiResponse PostCourse(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<CourseData>(json,
                                                                     setup.Business.UserName,
                                                                     setup.Business.Password,
                                                                     "Sessions");
        }

        protected ApiResponse WhenPostSession(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<SessionData>(json,
                                                                      setup.Business.UserName,
                                                                      setup.Business.Password,
                                                                      RelativePath);
        }

        protected ApiResponse WhenPostCourse(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<CourseData>(json,
                                                                     setup.Business.UserName,
                                                                     setup.Business.Password,
                                                                     RelativePath);
        }



        protected void AssertStatusCode(HttpStatusCode actualStatusCode, HttpStatusCode expectedStatusCode)
        {
            Assert.That(actualStatusCode, Is.EqualTo(expectedStatusCode));
        }

        protected void AssertNotFound(ApiResponse response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.NotFound);
        }

        protected void ThenReturnNotFound(ApiResponse response)
        {
            AssertNotFound(response);
        }

        protected void AssertUnauthorised(ApiResponse response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.Unauthorized);
        }

        protected T AssertSuccessResponse<T>(ApiResponse response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<T>());
            return (T)response.Payload;
        }

        protected void AssertSuccessResponse(ApiResponse response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);
        }

        protected ApiApplicationError[] AssertErrorResponse(ApiResponse response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApiApplicationError[]>());
            return (ApiApplicationError[])response.Payload;
        }

        protected ApiApplicationError AssertSingleError(ApiResponse response, string message, string field = null)
        {
            var errors = AssertErrorResponse(response);

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], field, message);

            return errors[0];
        }

        protected ApiApplicationError AssertSingleError(ApiResponse response, string code, string message, string data)
        {
            var errors = AssertErrorResponse(response);

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], code, message, data);

            return errors[0];
        }

        protected ApiApplicationError AssertSingleErrorContainsFragment(ApiResponse response, string code, string message, string dataFragment)
        {
            var errors = AssertErrorResponse(response);

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationErrorDataContainsFragment(errors[0], code, message, dataFragment);

            return errors[0];
        }

        protected void AssertMultipleErrors(ApiResponse response, string[,] expectedErrors)
        {
            var errors = AssertErrorResponse(response);
            Assert.That(errors.GetLength(0), Is.EqualTo(expectedErrors.GetLength(0)));

            var i = 0;
            foreach (var error in errors)
            {
                AssertApplicationError(error, expectedErrors[i, 0], expectedErrors[i, 1], expectedErrors[i, 2], expectedErrors[i, 3]);
                i++;
            }
        }

        protected void AssertApplicationError(ApiApplicationError error, string field, string message)
        {
            Assert.That(error.field, Is.EqualTo(field));
            Assert.That(error.message, Is.EqualTo(message));
        }

        protected void AssertApplicationError(ApiApplicationError error, string code, string message, string data, string field = null)
        {
            Assert.That(error.code, Is.EqualTo(code));
            Assert.That(error.message, Is.EqualTo(message));
            Assert.That(error.data, Is.EqualTo(data));
            Assert.That(error.field, Is.EqualTo(field));
        }

        protected void AssertApplicationErrorDataContainsFragment(ApiApplicationError error, string code, string message, string dataFragment)
        {
            Assert.That(error.code, Is.EqualTo(code));
            Assert.That(error.message, Is.EqualTo(message));
            Assert.That(error.data, Is.StringContaining(dataFragment));
        }
    }
}
