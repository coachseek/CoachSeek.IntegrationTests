﻿using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiServiceSaveCommand
    {
        public Guid? businessId { get; set; }
        public Guid? id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}