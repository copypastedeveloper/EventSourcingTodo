using System;
using MedArchon.Common.Commands.Bus;

namespace MedArchon.CommandBus
{
    public interface ICommandHandler
    {
        Type CommandType { get; }
        CommandResponse Handle(object command);
    }
}