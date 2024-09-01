using Application.Posts;
using Domain.AggegratesModel.NotificationAggegrate;

namespace Application.Notifications
{
    public class NotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public IEnumerable<Notification> GetNotifcations(int userId)
                => _notificationRepository.GetNotifcations(userId);

        public void AddNotification(Notification notification)
        {
            _notificationRepository.AddNotification(notification);
        }

        public void Save()
        {
            _notificationRepository.SaveChanges();
        }
    }
}
