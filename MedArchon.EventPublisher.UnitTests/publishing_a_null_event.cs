using System;
using Given.Common;
using Given.NUnit;
using log4net;
using MassTransit;
using Moq;

namespace MedArchon.EventPublisher.UnitTests
{
    public class publishing_a_null_event : Scenario
    {
        static Mock<IServiceBus> _bus;
        static Mock<ILog> _log;
        static EventPublisher _eventPublisher;
        static object _event;
        static Exception _exception;

        given an_event_publisher = () =>
        {
            _bus = new Mock<IServiceBus>();
            _log = new Mock<ILog>();
            _eventPublisher = new EventPublisher(_bus.Object,_log.Object);
        };

        given a_null_event = () => _event = null;

        when sending_an_event_to_the_publisher = () => _exception = Catch.Exception(() => _eventPublisher.PublishEvent(_event));

        [then]
        public void it_should_throw_an_argument_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [then]
        public void it_should_not_send_the_event_on_the_bus()
        {
            _bus.Verify(x => x.Publish(_event), Times.Never);
        }
    }
}