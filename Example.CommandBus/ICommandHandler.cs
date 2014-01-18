using System;
using Example.Common.Commands.Bus;

namespace Example.CommandBus
{
    public interface ICommandHandler
    {
        Type CommandType { get; }
        CommandResponse Handle(object command);
    }
}