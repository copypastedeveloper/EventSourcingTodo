using System;

namespace Example.Todo.Domain.Common.Exceptions
{
    public class UnknownEntityException : Exception
    {
        readonly Guid _id;

        public UnknownEntityException(Guid id, string message)
            : base(message)
        {
            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}