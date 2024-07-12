using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggegratesModel.UserAggegrate
{
    public sealed class User : Entity, IAggregateRoot
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Avatar { get; private set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenUpdated { get; private set; }

        private User()
        {
                
        }

        public User Create(string firstName, string lastName)
        {
            return new User()
            {
                FirstName = firstName,
                LastName = lastName,
                WhenUpdated = WhenUpdated
            };
        }
    }
}
