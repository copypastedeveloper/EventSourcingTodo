using System.Collections.Generic;
using System.Linq;
using MedArchon.Common.Commands.Bus;

namespace MedArchon.CommandBus
{
    public class SynchronousCommandBus : ICommandBus
    {
        readonly IEnumerable<ICommandHandler> _commandHandlers;

        public SynchronousCommandBus(IEnumerable<ICommandHandler> commandHandlers)
        {
            _commandHandlers = commandHandlers;
        }

        public CommandResponse Send(object command)
        {
            var handler = _commandHandlers.SingleOrDefault(x => x.CommandType == command.GetType());
            
            if (handler == null) throw new CommandHandlerForTypeNotFoundException(string.Format("Could not locate a handler for type {0}",command.GetType()));
            
            return handler.Handle(command);
        }
    }
}
