using Application.Notifications;
using Application.Posts;
using Domain.AggegratesModel.NotificationAggegrate;
using Domain.AggegratesModel.ActivityAggegrate;
using Domain.AggegratesModel.PostAggegrate.Events;
using Domain.AggegratesModel.UserAggegrate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Diagnostics;
using Activity = Domain.AggegratesModel.ActivityAggegrate.Activity;
using Application.Activites;

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
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var messages = ctx.OutBoxMessages.Where(x => x.WhenProcessed == null)
                                                     .Take(20)
                                                     .ToList();

                    foreach (var message in messages)
                    {
                        switch (message.Type)
                        {
                            case "COMMENT_CREATED":
                                var processors = scope.ServiceProvider.GetRequiredService<IEnumerable<ICommentCreatedProcessor>>();

                                foreach (var processor in processors)
                                {
                                    processor.Process(message);
                                }
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
}
