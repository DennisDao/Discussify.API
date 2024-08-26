namespace Discussify.API.DTOs.Post
{
    public class NewCommentRequest
    {
        public int UserId { get; set; }

        public int PostId { get; set; }

        public string Comment { get; set; }
    }
}
