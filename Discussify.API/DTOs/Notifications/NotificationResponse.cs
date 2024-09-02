namespace Discussify.API.DTOs.Notifications
{
    public class NotificationResponse
    {

        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string? Link { get;  set; }
        public string NotificationType { get; set; }
        public int EntityId { get; set; }
    }
}
