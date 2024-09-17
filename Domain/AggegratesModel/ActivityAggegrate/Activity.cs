using Domain.SeedWork;
using System;

namespace Domain.AggegratesModel.ActivityAggegrate
{
    public class Activity : Entity
    {
        public int UserId { get; private set; }
        public int CreateByUserId { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public ActivityType ActivityType { get; private set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenUpdated { get; private set; }

        private Activity()
        {

        }

        public static Activity Create(int userId, int createByUserId, string title, string content, ActivityType type)
        {
            Random random = new Random();
            return new Activity()
            {
                UserId = userId,
                CreateByUserId = createByUserId,
                Title = title,
                Content = content,
                ActivityType = type,
                WhenCreated = DateTime.UtcNow.AddDays(-random.Next(10)),
                WhenUpdated = DateTime.UtcNow
            };
        }
    }
}
