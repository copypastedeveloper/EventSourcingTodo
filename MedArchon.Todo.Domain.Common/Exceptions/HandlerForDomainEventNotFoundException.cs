using System;

namespace MedArchon.Todo.Domain.Common.Exceptions
{
    public class HandlerForDomainEventNotFoundException : Exception
    {
        public HandlerForDomainEventNotFoundException(string message)
            : base(message)
        {
        }
    }
}