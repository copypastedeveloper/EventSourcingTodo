using System;
using Given.Common;
using Given.NUnit;
using log4net;
using MassTransit;
using Moq;

namespace Example.EventPublisher.UnitTests
{
    public class publishing_an_event : Scenario
    {
        static Mock<IServiceBus> _bus;
        static Mock<ILog> _log;
        static EventPublisher _eventPublisher;
        static object _event;

        given an_event_publisher = () =>
        {
            _bus = new Mock<IServiceBus>();
            _log = new Mock<ILog>();
            _eventPublisher = new EventPublisher(_bus.Object, _log.Object);
        };

        given an_event = () => _event = "something";

        when sending_an_event_to_the_publisher = () => _eventPublisher.PublishEvent(_event);

        [then]
        public void it_should_send_the_event_on_the_bus()
        {
            _bus.Verify(x => x.Publish(_event, It.IsAny<Action<IPublishContext>>()), Times.Once);
        }
    }
}
