using System.Collections.Generic;
using Given.Common;
using Given.NUnit;
using Moq;

namespace Example.CommandBus.UnitTests
{
    public class handling_a_command_with_the_synchronous_command_bus : Scenario
    {
        static SynchronousCommandBus _bus;
        static Mock<ICommandHandler> _mockedStringCommandHandler;
        const string Command = "test";
        static Mock<ICommandHandler> _mockedIntCommandHandler;

        given a_synchronous_command_bus = () =>
        {
            _mockedStringCommandHandler = new Mock<ICommandHandler>();
            _mockedStringCommandHandler.Setup(x => x.CommandType).Returns(typeof (string));
            _mockedIntCommandHandler = new Mock<ICommandHandler>();
            _mockedIntCommandHandler.Setup(x => x.CommandType).Returns(typeof (int));

            _bus = new SynchronousCommandBus(new List<ICommandHandler> {_mockedStringCommandHandler.Object, _mockedIntCommandHandler.Object});
        };

        when sending_a_command_on_the_bus = () => _bus.Send(Command);

        [then]
        public void the_command_should_be_handled_by_the_proper_handler()
        {
            _mockedStringCommandHandler.Verify(x => x.Handle(Command), Times.Once());
        }

        [then]
        public void the_command_should_not_be_handled_by_improper_handlers()
        {
            _mockedIntCommandHandler.Verify(x => x.Handle(Command), Times.Never());
        }
    }
}
