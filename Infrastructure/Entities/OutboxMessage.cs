namespace Infrastructure.Entities
{
    public sealed class OutboxMessage
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public DateTime WhenCreated { get; set; }
        public DateTime? WhenProcessed { get; set; }
    }
}
