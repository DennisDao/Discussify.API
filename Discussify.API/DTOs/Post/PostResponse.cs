namespace Discussify.API.DTOs.Post
{
    public class PostResponse
    {
        public int UserId { get; set; }

        public int PostId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public List<PostComment> Comments { get; set; } = new List<PostComment>();

    }

    public class PostComment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }

        public int PostId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorImageUrl { get; set; }

        public string Content { get; set; }

        public string WhenCreated { get; set; }
    }
}
