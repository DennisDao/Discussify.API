using Domain.AggegratesModel.PostAggegrate.Events;
using Domain.AggegratesModel.PostAggegrate.Exceptions;
using Domain.SeedWork;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace Domain.AggegratesModel.PostAggegrate
{
    public class Post : Entity, IAggregateRoot
    {
        private readonly List<Tag> _tags = new List<Tag>();
        private readonly List<Comment> _comments = new List<Comment>();
        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string? Image { get; private set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenUpdated { get; private set; }
        public Category Category { get; private set; }
        public IEnumerable<Tag> Tags { get { return _tags; } }
        public IEnumerable<Comment> Comments { get { return _comments; } }

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

        public void ChangeImage(string image)
        {
            if(string.IsNullOrEmpty(image))
            {
                throw new PostInvalidStateException("Image is empty");
            }

            Image = image;
            WhenUpdated = DateTime.UtcNow;
        }

        public void SetCategory(Category category)
        {
            if (category is null)
            {
                throw new PostInvalidStateException("Category is empty");
            }

            Category = category;
            WhenUpdated = DateTime.UtcNow;
        }

        public void AddTags(Tag tag)
        {
            if (tag is null)
            {
                throw new PostInvalidStateException("Tag is empty");
            }

            _tags.Add(tag);
        }

        public void AddComment(Comment comment)
        {
            if (comment is null)
            {
                throw new PostInvalidStateException("Comment is empty");
            }

            _comments.Add(comment);

            CommentCreatedEvent commentCreated = new();
            commentCreated.UserId = comment.UserId;
            commentCreated.WhenCreated = DateTime.UtcNow;
            commentCreated.PostId = Id;

            AddDomainEvent(commentCreated);
        }

        public void Validate()
        {
            if(UserId is 0)
            {
                throw new PostInvalidStateException("Invalid user Id");
            }

            if (string.IsNullOrEmpty(Title))
            {
                throw new PostInvalidStateException("Title cannot be empty");
            }

            if (string.IsNullOrEmpty(Description))
            {
                throw new PostInvalidStateException("Description cannot be empty");
            }
        }
    }
}
