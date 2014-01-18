using System;
using System.Collections.Generic;
using System.Linq;
using EventStore;
using Given.Common;
using Given.NUnit;
using Example.Todo.Domain.Common;
using Moq;

namespace Example.Data.EventStore.UnitTests
{
    public class saving_an_entity : Scenario
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

        when saving_the_entity = () => _repository.Save(_mockedEntity.Object);

        [then]
        public void all_uncommitted_events_should_be_persisted()
        {
            _eventMessages.Select(x => x.Body).ShouldContainOnly(_eventQueue);
        }

        [then]
        public void all_events_should_be_applied_in_order()
        {
            _eventMessages.Select(x => x.Body).ShouldBeInTheSameOrderAs(_eventQueue);
        }

        [then]
        public void the_events_should_be_commited()
        {
            _mockedEventStream.Verify(y => y.CommitChanges(It.IsAny<Guid>()), Times.Once());
        }

        [then]
        public void there_should_be_no_uncommitted_events()
        {
            _mockedEntity.Verify(x => x.ClearUncommittedEvents(), Times.Once());
        }
    }
}