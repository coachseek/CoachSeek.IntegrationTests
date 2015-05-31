﻿using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class BusinessData
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string domain { get; set; }
        public string currency { get; set; }
        public string paymentProvider { get; set; }
        public string merchantAccountIdentifier { get; set; }
    }
}
