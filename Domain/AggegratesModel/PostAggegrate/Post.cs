using Domain.SeedWork;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace Domain.AggegratesModel.PostAggegrate
{
    public class Post : Entity, IAggregateRoot
    {
        private readonly List<Tag> _tags = new List<Tag>();
        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenUpdated { get; private set; }
        public Category Category { get; private set; }
        public IEnumerable<Tag> Tags { get { return _tags; } }

        private Post()
        {
            
        }

        public static Post Create(int userId, string title, string description)
        {
            return new Post
            {
                UserId = userId,
                Title = title,
                Description = description,
                WhenCreated = DateTime.UtcNow,
                WhenUpdated = DateTime.UtcNow,
            };
        }

        public void ChangeImage(string imageUrl)
        {
            ImageUrl = imageUrl;
            WhenUpdated = DateTime.UtcNow;
        }

        public void SetCategory(Category category)
        {
            Category = category;
            WhenUpdated = DateTime.UtcNow;
        }

        public void AddTags(Tag tag)
        {
            _tags.Add(tag);
        }
    }
}
