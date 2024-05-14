using Domain.SeedWork;
using System.ComponentModel.DataAnnotations;


namespace Domain.AggegratesModel.PostAggegrate
{
    public class Post : Entity, IAggregateRoot
    {
        public string UserId { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<Topic> Topics { get; private set; }

        public string ImageUrl { get; private set; }

        public DateTime WhenCreated { get; private set; }

        public DateTime WhenUpdated { get; private set; }

        public Post()
        {
            // Entity framework    
        }
    }
}
