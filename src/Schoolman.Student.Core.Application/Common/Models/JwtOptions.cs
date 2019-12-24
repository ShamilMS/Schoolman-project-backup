﻿using System;

namespace Schoolman.Student.Core.Application.Models
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public TimeSpan ExpirationTime { get; set; }
        public DateTime IssueDate { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
