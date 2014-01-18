using System;
using System.Collections.Generic;
using EventStore;
using Given.Common;
using Given.NUnit;
using Example.Todo.Domain.Common;
using Moq;

namespace Example.Data.EventStore.UnitTests
{
    public class saving_an_entity_with_failed_commit : Scenario
    {
        static Mock<IEventStream> _mockedEventStream;
        static Mock<IStoreEvents> _mockedEventStore;
        static EntityRepository _repository;
        static List<EventMessage> _eventMessages;
        static Queue<object> _eventQueue;
        const int MaxRevision = int.MaxValue;
        static readonly Guid EntityId = Guid.NewGuid();
        static Mock<IEntity> _mockedEntity;

        given an_event_store = () =>
        {
            _mockedEventStream = new Mock<IEventStream>();
            _eventMessages = new List<EventMessage>();

            _mockedEventStream.Setup(x => x.CommittedEvents).Returns(_eventMessages);
            _mockedEventStream.Setup(x => x.CommitChanges(It.IsAny<Guid>())).Throws<ConcurrencyException>();
            _mockedEventStream.Setup(x => x.Add(It.IsAny<EventMessage>()))
                .Callback<EventMessage>(message => _eventMessages.Add(message));

            _mockedEventStore = new Mock<IStoreEvents>();
            _mockedEventStore.Setup(x => x.OpenStream(EntityId, 0, MaxRevision))
                .Returns(_mockedEventStream.Object);

        };

        given an_entity_factory = () => _repository = new EntityRepository(_mockedEventStore.Object, new Mock<IEntityFactory>().Object);

        given an_entity_with_uncommitted_events = () =>
        {
            _mockedEntity = new Mock<IEntity>();
            _mockedEntity.Setup(x => x.Id).Returns(EntityId);
            
            _eventQueue = new Queue<object>(new List<object>
            {
                new SomeDomainEvent(),
                new SomeDomainEvent(),
                new AnotherDomainEvent()
            });

            _mockedEntity.Setup(x => x.UncommittedEvents()).Returns(_eventQueue);
        };

        when saving_the_entity = () => Catch.Exception(() => _repository.Save(_mockedEntity.Object));

        [then]
        public void the_uncommitted_events_should_not_be_cleared()
        {
            _mockedEntity.Verify(x => x.ClearUncommittedEvents(), Times.Never);
        }
    }
}