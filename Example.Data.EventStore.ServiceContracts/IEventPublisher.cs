namespace Example.Data.EventStore.ServiceContracts
{
    public interface IEventPublisher
    {
        void PublishEvent(object eventToPublish);
    }
}