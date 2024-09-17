using Application.Activites;
using Application.Posts;
using Domain.AggegratesModel.ActivityAggegrate;
using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.PostAggegrate.Events;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Infrastructure.BackroundServices
{
    public class ActivityCommentCreatedProcessor : ICommentCreatedProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        public ActivityCommentCreatedProcessor(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public async void Process(OutboxMessage message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var postService = scope.ServiceProvider.GetRequiredService<PostService>();
                var activityRepo = scope.ServiceProvider.GetRequiredService<IActivityRepository>();
                var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();


                var messageEvent = JsonConvert.DeserializeObject<CommentCreatedEvent>(message.Content);
                var post = postService.GetPost(messageEvent.PostId);

                var createdByUserId = await userRepo.GetUserByIdAsync(messageEvent.UserId);

                var activity = Activity.Create(post.UserId,
                messageEvent.UserId,
                                          $"{(post.UserId == messageEvent.UserId ? "You" : createdByUserId.FirstName)} commented on your post",
                                          "test",
                                          ActivityType.CommentCreated);

                activityRepo.Add(activity);

            }
           
        }
    }
}
