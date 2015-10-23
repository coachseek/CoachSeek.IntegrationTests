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


        protected SetupData RegisterExpiredBusinessIfNotExists()
        {
            var expiredBusiness = new ExpectedBusiness("Expired Business",
                                                       "Tennis",
                                                       "NZD",
                                                       "Test",
                                                       "Tester",
                                                       "expired.business@coachseek.com",
                                                       "666 666",
                                                       "password");
            var getResponse = BusinessAnonymousGet<BusinessData>("OnlineBooking/Business", "expiredbusiness");
            if (getResponse.StatusCode == HttpStatusCode.OK)
                return new SetupData(expiredBusiness);
            var response = BusinessRegistrar.RegisterBusiness(expiredBusiness);
            return new SetupData((RegistrationData)response.Payload, expiredBusiness.Password);
        }

        protected SetupData RegisterBusiness()
        {
            var business = new RandomBusiness();
            var response = BusinessRegistrar.RegisterBusiness(business);
            return new SetupData((RegistrationData)response.Payload, business.Password);
        }


        protected ApiResponse PostBooking(string json, SetupData setup)
        {
            return AuthenticatedPost<SingleSessionBookingData>(json, "Bookings", setup);
        }

        protected ApiResponse PostCourseBooking(string json, SetupData setup)
        {
            return AuthenticatedPost<CourseBookingData>(json, "Bookings", setup);
        }


        protected ApiResponse BusinessAnonymousGet<TResponse>(string relativePath, SetupData setup)
        {
            return BusinessAnonymousGet<TResponse>(relativePath, setup.Business.Domain);
        }

        protected ApiResponse BusinessAnonymousGet<TResponse>(string relativePath, Guid id, SetupData setup)
        {
            return BusinessAnonymousGet<TResponse>(relativePath, id, setup.Business.Domain);
        }

        protected ApiResponse BusinessAnonymousGet<TResponse>(string relativePath, string businessDomain)
        {
            //return new TestCoachseekBusinessAnonymousApiClient(businessDomain).Get<TResponse>(relativePath);
            return new TestCoachseekBusinessAnonymousApiClient(businessDomain).GetAsync<TResponse, ApiApplicationError[]>(relativePath).Result;
        }

        protected ApiResponse BusinessAnonymousGet<TResponse>(string relativePath, Guid id, string businessDomain)
        {
            return BusinessAnonymousGet<TResponse>(string.Format("{0}/{1}", relativePath, id), businessDomain);
        }


        protected ApiResponse AuthenticatedGet<TResponse>(string relativePath, SetupData setup)
        {
            return AuthenticatedGet<TResponse>(relativePath, setup.Business.UserName, setup.Business.Password);
        }

        protected ApiResponse AuthenticatedGet<TResponse>(string relativePath, Guid id, SetupData setup)
        {
            return AuthenticatedGet<TResponse>(relativePath, id, setup.Business.UserName, setup.Business.Password);
        }

        protected ApiResponse AuthenticatedGet<TResponse>(string relativePath, string username, string password)
        {
            return new TestCoachseekAuthenticatedApiClient(username, password)
                        .Get<TResponse>(relativePath);
        }

        protected ApiResponse AuthenticatedGet<TResponse>(string relativePath, Guid id, string username, string password)
        {
            return AuthenticatedGet<TResponse>(string.Format("{0}/{1}", relativePath, id), username, password);
        }

        protected ApiResponse AdminGet<TResponse>(string relativePath)
        {
            return new TestCoachseekAdminApiClient().Get<TResponse>(relativePath);
        }


        protected ApiResponse AnonymousPost<TResponse>(string json, string relativePath)
        {
            return new TestCoachseekAnonymousApiClient().Post<TResponse>(json, relativePath);
        }

        protected ApiResponse BusinessAnonymousPost<TResponse>(string json, string relativePath, SetupData setup)
        {
            return BusinessAnonymousPost<TResponse>(json, relativePath, setup.Business.Domain);
        }

        protected ApiResponse BusinessAnonymousPost<TResponse>(string json, string relativePath, string businessDomain)
        {
            return new TestCoachseekBusinessAnonymousApiClient(businessDomain).Post<TResponse>(json, relativePath);
        }

        protected ApiResponse AuthenticatedPost<TResponse>(string json, string relativePath, SetupData setup)
        {
            return AuthenticatedPost<TResponse>(json, relativePath, setup.Business.UserName, setup.Business.Password);
        }

        protected ApiResponse AuthenticatedPost<TResponse>(string json, string relativePath, string username, string password)
        {
            //return new TestCoachseekAuthenticatedApiClient(username, password).Post<TResponse>(json, relativePath);
            return new TestCoachseekAuthenticatedApiClient(username, password)
                        .PostAsync<TResponse, ApiApplicationError[]>(json, relativePath).Result;
        }

        protected ApiResponse AdminPost(string json, string relativePath)
        {
            return new TestCoachseekAdminApiClient().Post(json, relativePath);
        }


        protected ApiResponse AnonymousDelete(string relativePath, string id)
        {
            return new TestCoachseekAnonymousApiClient().Delete(relativePath, id);
        }

        protected ApiResponse AuthenticatedDelete(string relativePath, string id, SetupData setup)
        {
            return new TestCoachseekAuthenticatedApiClient(setup.Business.UserName, setup.Business.Password)
                        .Delete(relativePath, id);
        }


        protected ApiResponse PostSession(string json, SetupData setup)
        {
            return AuthenticatedPost<SessionData>(json, "Sessions", setup);
        }

        protected ApiResponse PostCourse(string json, SetupData setup)
        {
            return AuthenticatedPost<CourseData>(json, "Sessions", setup);
        }

        protected ApiResponse WhenPostSession(string json, SetupData setup)
        {
            return AuthenticatedPost<SessionData>(json, RelativePath, setup);
        }

        protected ApiResponse WhenPostCourse(string json, SetupData setup)
        {
            return AuthenticatedPost<CourseData>(json, RelativePath, setup);
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

        protected ApiApplicationError AssertSingleError(ApiResponse response, string code, string message, string data = null)
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
                AssertApplicationError(error, expectedErrors[i, 0], expectedErrors[i, 1], expectedErrors[i, 2]);
                i++;
            }
        }

        protected void AssertApplicationError(ApiApplicationError error, string code, string message, string data = null)
        {
            Assert.That(error.code, Is.EqualTo(code));
            Assert.That(error.message, Is.EqualTo(message));
            Assert.That(error.data, Is.EqualTo(data));
        }

        protected void AssertApplicationErrorDataContainsFragment(ApiApplicationError error, string code, string message, string dataFragment)
        {
            Assert.That(error.code, Is.EqualTo(code));
            Assert.That(error.message, Is.EqualTo(message));
            Assert.That(error.data, Is.StringContaining(dataFragment));
        }

        protected void AssertDateTime(DateTime actualDateTime, DateTime expectedDateTime, int varianceInSeconds = 60)
        {
            Assert.That(actualDateTime, Is.GreaterThan(expectedDateTime.AddSeconds(-1 * varianceInSeconds)));
            Assert.That(actualDateTime, Is.LessThan(expectedDateTime.AddSeconds(varianceInSeconds)));
        }
    }
}
