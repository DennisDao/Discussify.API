using Application.Notifications;
using Domain.AggegratesModel.NotificationAggegrate;
using Domain.AggegratesModel.UserAggegrate;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
        }

        public Notification GetNotifcationById(int notifcationId)
        {
            return _context.Notifications.FirstOrDefault(x => x.Id == notifcationId);
        }

        public IEnumerable<Notification> GetNotifcations(int userId)
        {
            return _context.Notifications.Where(x => x.UserId == userId && x.IsViewed == false);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
