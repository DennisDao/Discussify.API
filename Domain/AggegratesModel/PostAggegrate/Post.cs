using Domain.SeedWork;
using System.ComponentModel.DataAnnotations;


namespace Domain.AggegratesModel.PostAggegrate
{
    public class Post : Entity, IAggregateRoot
    {
        public int UserId { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<Topic> Topics { get; private set; }

        public string ImageUrl { get; private set; }

        public DateTime WhenCreated { get; private set; }

        public DateTime WhenUpdated { get; private set; }

        private Post()
        {
            
        }

        public static Post Create(int userId, string title, string description, IEnumerable<Topic> topics)
        {
            return new Post
            {
                UserId = userId,
                Title = title,
                Description = description,
                Topics = topics,
                WhenCreated = DateTime.UtcNow,
                WhenUpdated = DateTime.UtcNow,
            };
        }

        public void ChangeImage(string imageUrl)
        {
            ImageUrl = imageUrl;
            WhenUpdated = DateTime.UtcNow;
        }
    }
}
