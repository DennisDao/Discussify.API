using Domain.SeedWork;

namespace Domain.AggegratesModel.PostAggegrate.Events
{
    public class CommentCreatedEvent : IDomainEvent
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime WhenCreated { get; set; }
        public string EventType => "COMMENT_CREATED";
    }
}
