namespace Discussify.API.DTOs.Notifications
{
    public class NotificationResponse
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public string? Link { get;  set; }
    }
}
