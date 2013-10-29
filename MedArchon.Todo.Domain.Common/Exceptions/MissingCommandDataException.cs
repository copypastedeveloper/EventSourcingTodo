using System;

namespace MedArchon.Todo.Domain.Common.Exceptions
{
    public class MissingCommandDataException : Exception
    {
        public MissingCommandDataException(string message)
            : base(message)
        {
        }
    }
}