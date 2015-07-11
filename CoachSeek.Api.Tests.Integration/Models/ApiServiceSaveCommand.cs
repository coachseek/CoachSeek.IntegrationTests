using System;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Service;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiServiceSaveCommand
    {
        public Guid? id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public ApiServiceTiming timing { get; set; }
        public ApiServiceBooking booking { get; set; }
        public ApiPresentation presentation { get; set; }
        public ApiServiceRepetition repetition { get; set; }
        public ApiPricing pricing { get; set; }


        public ApiServiceSaveCommand()
        { }

        public ApiServiceSaveCommand(ExpectedService service)
        {
            id = service.Id;
            name = service.Name;
            description = service.Description;
            timing = service.Timing;
            booking = service.Booking;
            presentation = service.Presentation;
            repetition = service.Repetition;
            pricing = service.Pricing;
        }
    }
}
