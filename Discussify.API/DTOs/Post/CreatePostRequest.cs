namespace Discussify.API.DTOs.Post
{
    public class CreatePostRequest
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
