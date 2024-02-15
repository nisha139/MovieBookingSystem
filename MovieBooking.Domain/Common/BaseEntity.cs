using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        private readonly List<BaseEvent> _domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
// These classes demonstrate how to encapsulate common functionality in a base class,
// which is a key principle of DDD(Domain-Driven Design).
//The [NotMapped] attribute is used to instruct EF that the DomainEvents property should not be mapped to any database column.