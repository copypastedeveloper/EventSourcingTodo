using System;
using System.Collections.Generic;
using Given.Common;
using Given.NUnit;
using Moq;

namespace MedArchon.CommandBus.UnitTests
{
    public class handling_a_command_with_no_handler : Scenario
    {
        static SynchronousCommandBus _bus;
        const string Command = "test";
        static Mock<ICommandHandler> _mockedIntCommandHandler;
        static Exception _exception;

        given a_synchronous_command_bus = () =>
        {
            _mockedIntCommandHandler = new Mock<ICommandHandler>();
            _mockedIntCommandHandler.Setup(x => x.CommandType).Returns(typeof (int));

            _bus = new SynchronousCommandBus(new List<ICommandHandler> {_mockedIntCommandHandler.Object});
        };

        when sending_a_command_on_the_bus = () => _exception = Catch.Exception(() => _bus.Send(Command));

        [then]
        public void the_command_should_not_be_handled_by_improper_handlers()
        {
            _mockedIntCommandHandler.Verify(x => x.Handle(Command), Times.Never());
        }
        
        [then]
        public void a_handler_not_found_error_should_be_thrown()
        {
            _exception.ShouldBeOfType<CommandHandlerForTypeNotFoundException>();
        }
    }
}