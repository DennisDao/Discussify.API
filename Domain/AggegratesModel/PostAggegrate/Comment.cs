using Domain.SeedWork;

namespace Domain.AggegratesModel.PostAggegrate
{
    public class Comment : Entity
    {
        public int UserId { get; private set; }
        public int PostId { get; private set; }
        public string Content { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenUpdated { get; private set; }
        private Comment() 
        { 

        }

        public static Comment Create(int userId, int postId, string content)
        {
            return new Comment
            {
                UserId = userId,
                Content = content,
                WhenCreated = DateTime.UtcNow,
                WhenUpdated = DateTime.UtcNow,
            };
        }
    }
}
