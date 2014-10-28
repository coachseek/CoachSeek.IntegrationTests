using System.Net;

namespace CoachSeek.Api.Tests.Integration.Models
{

    public class Response
    {
        public HttpStatusCode StatusCode { get; private set; }
        public object Payload { get; private set; }

        public Response(HttpStatusCode statusCode, object payload)
        {
            StatusCode = statusCode;
            Payload = payload;
        }
    }
}
