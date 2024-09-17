using Application.Notifications;
using Application.Posts;
using Domain.AggegratesModel.NotificationAggegrate;
using Domain.AggegratesModel.PostAggegrate.Events;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static System.Formats.Asn1.AsnWriter;

namespace Infrastructure.BackroundServices
{
    public class NotificationProcessor : ICommentCreatedProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        public NotificationProcessor(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public async void Process(OutboxMessage message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var postService = scope.ServiceProvider.GetRequiredService<PostService>();
                var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
                var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                var messageEvent = JsonConvert.DeserializeObject<CommentCreatedEvent>(message.Content);
                var post = postService.GetPost(messageEvent.PostId);

                var distinctUserIds = post.Comments
                                          .Where(comment => comment.UserId != messageEvent.UserId)
                                          .Select(comment => comment.UserId)
                                          .Distinct()
                                          .ToList();

                var createdByUserId = await userRepo.GetUserByIdAsync(messageEvent.UserId);

                foreach (var userId in distinctUserIds)
                {
                    var notification = Notification.Create(userId, $"{createdByUserId.FirstName} commented on {post.Title}", "", NotificationEntityType.Post, post.Id);
                    notificationService.AddNotification(notification);
                }

                notificationService.Save();
            }
        }
    }
}
