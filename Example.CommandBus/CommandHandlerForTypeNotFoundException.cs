using System;

namespace Example.CommandBus
{
    public class CommandHandlerForTypeNotFoundException : Exception
    {
         public CommandHandlerForTypeNotFoundException(string message)
            : base(message)
        {
        }
    }
}