using System;
using Example.Data.EventStore.ServiceContracts;
using log4net;
using MassTransit;

namespace Example.EventPublisher
{
    public class EventPublisher : IEventPublisher
    {
        readonly IServiceBus _bus;
        readonly ILog _log;

        public EventPublisher(IServiceBus bus, ILog log)
        {
            _bus = bus;
            _log = log;
        }

        public void PublishEvent(object eventToPublish)
        {
            if (eventToPublish == null) throw new ArgumentNullException("eventToPublish");

            _bus.Publish(eventToPublish, (IPublishContext context) =>
            {
                context.IfNoSubscribers(() => _log.Warn(string.Format("No subscribers for message of type {0}", context.MessageType)));
                context.ForEachSubscriber(x => _log.Info(string.Format("message of type {0} will be sent to {1}", context.MessageType, x.Uri)));
            });
        }
    }
}
