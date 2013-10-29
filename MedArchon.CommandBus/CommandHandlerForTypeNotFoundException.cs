using System;

namespace MedArchon.CommandBus
{
    public class CommandHandlerForTypeNotFoundException : Exception
    {
         public CommandHandlerForTypeNotFoundException(string message)
            : base(message)
        {
        }
    }
}