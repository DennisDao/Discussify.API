﻿namespace Api.DTOs.Authors
{
    public class AuthorResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }

        public string Bio { get; set; }

        public int TotalFollowers { get; set; }

        public int TotalPost { get; set; }

        public bool IsFollowing { get; set; }
    }
}
