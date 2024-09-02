using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggegratesModel.NotificationAggegrate
{
    public class Notification : Entity
    {
        public int UserId { get; private set; }
        public string Message { get; private set; }
        public bool IsViewed { get; private set; }
        public string? Link { get; private set; }
        public int EntityId { get; private set; }
        public NotificationEntityType NotificationEntityType { get; private set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenUpdated { get; private set; }

        private Notification()
        {
                
        }

        public static Notification Create(int userId, string message, string? link, NotificationEntityType type, int entityId)
        {
            return new Notification () 
            {   
                UserId = userId,
                Message = message,
                Link = link,
                EntityId = entityId,
                NotificationEntityType = type,
                WhenCreated = DateTime.UtcNow,
                WhenUpdated = DateTime.UtcNow,
            };
        }

        public void SetViewed()
        {
            IsViewed = true;
        }
    }
}
