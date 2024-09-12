namespace Api.DTOs.Authors
{
    public class FollowAuthorRequest
    {
        public int UserId { get; set; }

        public int FollowingUserId { get; set; }
    }
}
