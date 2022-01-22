﻿using System.ComponentModel.DataAnnotations;

namespace TweetBook2.Contracts.V1.Requests
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
