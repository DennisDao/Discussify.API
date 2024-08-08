namespace Discussify.API.DTOs.Post
{
    public class PostCreateRequest
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public List<string> Tags { get; set; }
    }
}
