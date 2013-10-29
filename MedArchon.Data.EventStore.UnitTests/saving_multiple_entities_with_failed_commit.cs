using System;
using System.Collections.Generic;
using System.Transactions;
using EventStore;
using Given.Common;
using Given.NUnit;
using MedArchon.Todo.Domain.Common;
using Moq;

namespace MedArchon.Data.EventStore.UnitTests
{
    public class saving_multiple_entities_with_failed_commit : Scenario
    {
        static Mock<IEventStream> _mockedEventStream;
        static Mock<IStoreEvents> _mockedEventStore;
        static EntityRepository _repository;
        static List<EventMessage> _eventMessages;
        static Queue<object> _eventQueue;
        const int MaxRevision = int.MaxValue;
        static readonly Guid LastEntityId = Guid.NewGuid();
        static List<Mock<IEntity>> _entities;

        given an_event_store = () =>
        {
            _mockedEventStream = new Mock<IEventStream>();
            _eventMessages = new List<EventMessage>();

            _mockedEventStream.Setup(x => x.CommittedEvents).Returns(_eventMessages);
            _mockedEventStream.Setup(x => x.CommitChanges(LastEntityId)).Throws<ConcurrencyException>();
            _mockedEventStream.Setup(x => x.Add(It.IsAny<EventMessage>()))
                .Callback<EventMessage>(message => _eventMessages.Add(message));

            _mockedEventStore = new Mock<IStoreEvents>();
            _mockedEventStore.Setup(x => x.OpenStream(It.IsAny<Guid>(), 0, MaxRevision))
                .Returns(_mockedEventStream.Object);

        };

        given an_entity_factory = () => _repository = new EntityRepository(_mockedEventStore.Object, new Mock<IEntityFactory>().Object);

        given a_entities_with_uncommitted_events = () =>
        {
            _entities = new List<Mock<IEntity>>();
            for (var i = 0; i < 5; i++)
            {
                _entities.Add(CreateEntityMock(Guid.NewGuid()));
            }
        };

        when saving_the_entities_in_a_transaction = () =>
        {
            using (var tx = new TransactionScope())
            {
                _entities.ForEach(mockedEntity => Catch.Exception(() => _repository.Save(mockedEntity.Object)));
            }
        };

        [then]
        public void the_uncommitted_events_should_not_be_cleared_on_any_entity()
        {
            _entities.ForEach(mockedEntity => mockedEntity.Verify(x => x.ClearUncommittedEvents(), Times.Never));
        }

        static Mock<IEntity> CreateEntityMock(Guid entityId)
        {
            var mockedEntity = new Mock<IEntity>();
            mockedEntity.Setup(x => x.Id).Returns(entityId);

            _eventQueue = new Queue<object>(new List<object>
            {
                new SomeDomainEvent(),
                new SomeDomainEvent(),
                new AnotherDomainEvent()
            });

            mockedEntity.Setup(x => x.UncommittedEvents()).Returns(_eventQueue);
            return mockedEntity;
        }
    }
}