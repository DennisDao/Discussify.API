﻿using MediatR;

namespace Domain.SeedWork
{
    public abstract class Entity
    {
        private int? _requestedHashCode;
        private int _Id;
        private List<IDomainEvent> _domainEvents;
        public virtual int Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }
        public List<IDomainEvent> DomainEvents => _domainEvents;
        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }
        public void ClearDomainEvent() 
        {
            _domainEvents.Clear();
        }
        public bool IsTransient()
        {
            return this.Id == default(Int32);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;
            Entity item = (Entity)obj;
            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;
                // XOR for random distribution. See:
                // https://learn.microsoft.com/archive/blogs/ericlippert/guidelines-and-rulesfor-gethashcode
                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }
        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null));
            else
                return left.Equals(right);
        }
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
