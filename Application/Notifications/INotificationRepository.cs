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

        /// <summary> Get notifcation by Id</summary>
        Notification GetNotifcationById(int notifcationId);

        /// <summary>Add a notifcation </summary>
        void AddNotification(Notification notification);

        /// <summary>Commit the changes</summary>
        void SaveChanges();
    }
}
