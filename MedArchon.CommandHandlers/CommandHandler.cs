using System;
using MedArchon.CommandBus;
using MedArchon.Common.Commands.Bus;

namespace MedArchon.CommandHandlers
{
    public abstract class CommandHandler<T> : ICommandHandler where T: class
    {
        public Type CommandType 
        {
            get { return typeof (T); }
        }

        protected abstract CommandResponse Handle(T command);

        public CommandResponse Handle(object command)
        {
            if (command == null) throw new ArgumentNullException("command");

            var typedCommand = command as T;
            if (typedCommand == null) throw new ArgumentException("command");

            //some basic command validation should be done in here.
            
            return Handle(typedCommand);
        }
    }
}