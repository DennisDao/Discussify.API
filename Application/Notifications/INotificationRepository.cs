using Domain.AggegratesModel.NotificationAggegrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications
{
    public interface INotificationRepository
    {
        /// <summary> Get all notifcation for a user</summary>
        IEnumerable<Notification> GetNotifcations(int userId);
    }
}
