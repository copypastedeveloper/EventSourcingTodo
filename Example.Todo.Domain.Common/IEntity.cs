using System;
using System.Collections.Generic;

namespace Example.Todo.Domain.Common
{
    public interface IEntity
    {
        // todo What should access level be for the setter of Id.
        Guid Id { get; set; }
        void ApplyEvent(object @event);
        Queue<object> UncommittedEvents();
        void ClearUncommittedEvents();
    }
}