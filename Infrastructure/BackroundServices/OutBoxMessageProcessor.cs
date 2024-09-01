using Application.Notifications;
using Application.Posts;
using Domain.AggegratesModel.NotificationAggegrate;
using Domain.AggegratesModel.PostAggegrate.Events;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Infrastructure.BackroundServices
{
    public class OutBoxMessageProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public OutBoxMessageProcessor(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

                var scope = _serviceProvider.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var postService = scope.ServiceProvider.GetRequiredService<PostService>();
                var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
                var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                var messages = ctx.OutBoxMessages
                                            .Where(x => x.WhenProcessed == null)
                                            .Take(20)
                                            .ToList();

                foreach(var message in messages) 
                {
                    switch (message.Type)
                    {
                        case "COMMENT_CREATED":
                            var commentCreated = JsonConvert.DeserializeObject<CommentCreatedEvent>(message.Content);
                            var post = postService.GetPost(commentCreated.PostId);

                            var userIds = postService
                                .GetPost(commentCreated.PostId)
                                .Comments
                                .Where(x => x.UserId != commentCreated.UserId)
                                .Select(x => x.UserId)
                                .Distinct()
                                .ToList();

                            foreach (var userId in userIds)
                            {
                                var user = await userRepo.GetUserByIdAsync(commentCreated.UserId);
                                var notification = Notification.Create(user.Id, $"{user.FirstName} commented {post.Title}", "www.google.com");
                                notificationService.AddNotification(notification);
                            }

                            notificationService.Save();
                            break;
                        default:
                            break;
                    }

                    message.WhenProcessed = DateTime.UtcNow;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
