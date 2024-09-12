namespace Api.DTOs.Authors
{
    public class FollowAuthorResponse
    {
        public int UserId { get; set; }

        public int FollowerId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }

        public string Bio { get; set; }

        public bool IsFollowing { get; set; }
    }
}
