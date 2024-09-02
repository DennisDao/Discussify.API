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
        {
            return _notificationRepository.GetNotifcations(userId);
        }

        public Notification GetNotificationById(int notificationId)
        {
           return _notificationRepository.GetNotifcationById(notificationId);
        }
         
        public void AddNotification(Notification notification)
        {
            _notificationRepository.AddNotification(notification);
        }

        public void SetNotificationToViewed(int notificationId)
        {
           var notification = _notificationRepository.GetNotifcationById(notificationId);
           notification.SetViewed();
           Save();
        }

        public void Save()
        {
            _notificationRepository.SaveChanges();
        }
    }
}
