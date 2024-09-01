using Application.Notifications;
using Domain.AggegratesModel.NotificationAggegrate;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Notification> GetNotifcations(int userId)
        {
            return _context.Notifications.Where(x => x.UserId == userId);
        }
    }
}
