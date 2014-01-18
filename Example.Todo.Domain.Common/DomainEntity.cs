using System;
using System.Collections.Generic;

namespace Example.Todo.Domain.Common
{
    public abstract class DomainEntity : IEntity, IDisposable
    {
        IEventRouter _registeredRoutes;
        readonly Queue<object> _uncommittedEvents;

        protected DomainEntity()
        {
            _uncommittedEvents = new Queue<object>();
        }

        protected virtual IEventRouter RegisteredRoutes
        {
            get { return _registeredRoutes ?? (_registeredRoutes = new ConventionEventRouter(this)); }
        }

        public Guid Id { get; set; }

        public void ApplyEvent(object @event)
        {
            RegisteredRoutes.Dispatch(@event);
        }

        public Queue<object> UncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        protected void RaiseEvent(object @event)
        {
            ApplyEvent(@event);
            _uncommittedEvents.Enqueue(@event);
        }

        public void Dispose()
        {
            _uncommittedEvents.Clear();
        }
    }
}