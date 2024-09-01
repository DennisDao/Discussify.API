using Domain.AggegratesModel.UserAggegrate;
using Domain.SeedWork;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Infrastructure.Interceptors
{
    public sealed class DomainEventsInterceptor : SaveChangesInterceptor
    {
        public DomainEventsInterceptor() 
        {

        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            var dbContext = eventData.Context;

            if (dbContext == null)
            {
                return base.SavedChanges(eventData, result);
            }

            var entities = dbContext.ChangeTracker
               .Entries<IAggregateRoot>()
               .Select(x => x.Entity);

             var events = entities.SelectMany(aggregateRoot =>
               {
                   var entity = (aggregateRoot as Entity);
                   var domainEvents = entity?.DomainEvents;
                   return domainEvents;
               }).ToList();

            if (!events.Any())
            {
                return base.SavedChanges(eventData, result);
            }

            // Add to outbox message
            var outboxMessages = events.Select(domainEvent => new OutboxMessage()
            {
                WhenCreated = DateTime.UtcNow,
                Type = domainEvent.EventType,
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All})
            }).ToList();

            foreach(var entity in entities)
            {
                (entity as Entity).ClearDomainEvent();
            }

            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

            return dbContext.SaveChanges();
        }
    }
}
