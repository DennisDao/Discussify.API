﻿namespace Api.DTOs.Authors
{
    public class AuthorResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }
    }
}
