using System;
using System.Collections.Generic;
using System.Linq;
using EventStore;
using Given.Common;
using Given.NUnit;
using MedArchon.Todo.Domain.Common;
using Moq;

namespace MedArchon.Data.EventStore.UnitTests
{
    public class constructing_an_entity_by_id_specifying_a_version : Scenario
    {
        static Mock<IStoreEvents> _mockedEventStore;
        static Mock<IEntityFactory> _mockedEntityFactory;
        static EntityRepository _repository;
        static List<EventMessage> _eventMessages;
        const int MaxRevision = 4;
        static readonly Guid EntityId = Guid.NewGuid();
        static IEntity _entity;
        static readonly List<object> AppliedEvents = new List<object>();

        given an_event_store = () =>
                                   {
                                       var mockedEventStream = new Mock<IEventStream>();
                                       _eventMessages = new List<EventMessage>
                                                            {
                                                                new EventMessage {Body = new SomeDomainEvent {Value = "test"}}, 
                                                                new EventMessage {Body = new SomeDomainEvent {Value = "something"}}, 
                                                                new EventMessage {Body = new AnotherDomainEvent {Value = DateTime.Parse("1/1/2001")}}, 
                                                                new EventMessage {Body = new AnotherDomainEvent {Value = DateTime.Parse("1/1/2002")}},
                                                            };

                                       mockedEventStream.Setup(x => x.CommittedEvents).Returns(_eventMessages);
                                       _mockedEventStore = new Mock<IStoreEvents>();
                                       _mockedEventStore.Setup(x => x.OpenStream(EntityId, 0, MaxRevision))
                                                        .Returns(mockedEventStream.Object);

                                       _mockedEntityFactory = new Mock<IEntityFactory>();
                                       var mockedEntity = new Mock<IEntity>();
                                       mockedEntity.Setup(x => x.ApplyEvent(It.IsAny<object>()))
                                           .Callback<object>(x => AppliedEvents.Add(x));

                                       _mockedEntityFactory.Setup(x => x.Create<IEntity>())
                                           .Returns(mockedEntity.Object);
                                   };

        given an_entity_factory = () => _repository = new EntityRepository(_mockedEventStore.Object, _mockedEntityFactory.Object);

        when constructing_entity_by_id = () => _entity = _repository.GetById<IEntity>(EntityId, 4);

        [then]
        public void events_should_be_read_from_the_store_specifying_the_version()
        {
            _mockedEventStore.Verify(x => x.OpenStream(EntityId,0,MaxRevision));
        }

        [then]
        public void all_events_should_be_applied()
        {
            AppliedEvents.ShouldContainOnly(_eventMessages.Select(x => x.Body).ToList());
        }

        [then]
        public void all_events_should_be_applied_in_order()
        {
            AppliedEvents.ShouldBeInTheSameOrderAs(_eventMessages.Select(x => x.Body).ToList());
        }
    }
}