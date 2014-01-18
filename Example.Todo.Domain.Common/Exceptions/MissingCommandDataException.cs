using System;

namespace Example.Todo.Domain.Common.Exceptions
{
    public class MissingCommandDataException : Exception
    {
        public MissingCommandDataException(string message)
            : base(message)
        {
        }
    }
}