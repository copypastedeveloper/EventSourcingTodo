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
    public class constructing_an_entity_by_id : Scenario
    {
        static Mock<IStoreEvents> _mockedEventStore;
        static EntityRepository _repository;
        static List<EventMessage> _eventMessages;
        static readonly Guid EntityId = Guid.NewGuid();
        static Mock<IEntity> _mockedEntity;
        static readonly List<object> AppliedEvents = new List<object>();
        static Mock<IEntityFactory> _mockedEntityFactory;
        static IEntity _result;

        given an_event_store = () =>
                                   {
                                       var mockedEventStream = new Mock<IEventStream>();
                                       _eventMessages = new List<EventMessage>
                                                               {
                                                                   new EventMessage {Body = new SomeDomainEvent {Value = "test"}}, 
                                                                   new EventMessage {Body = new SomeDomainEvent {Value = "something"}}, 
                                                                   new EventMessage {Body = new AnotherDomainEvent {Value = DateTime.Now}}, 
                                                                   new EventMessage {Body = new AnotherDomainEvent {Value = DateTime.Parse("1/1/2001")}},
                                                               };

                                       mockedEventStream.Setup(x => x.CommittedEvents).Returns(_eventMessages);
                                       _mockedEventStore = new Mock<IStoreEvents>();
                                       _mockedEventStore.Setup(x => x.OpenStream(EntityId, 0, int.MaxValue))
                                                        .Returns(mockedEventStream.Object);
                                       _mockedEntityFactory = new Mock<IEntityFactory>();
                                       _mockedEntity = new Mock<IEntity>();
                                       _mockedEntity.Setup(x => x.ApplyEvent(It.IsAny<object>()))
                                           .Callback<object>(x => AppliedEvents.Add(x));

                                       _mockedEntityFactory.Setup(x => x.Create<IEntity>())
                                           .Returns(_mockedEntity.Object);
                                   };

        given an_entity_factory = () => _repository = new EntityRepository(_mockedEventStore.Object, _mockedEntityFactory.Object);

        when constructing_entity_by_id = () => _result = _repository.GetById<IEntity>(EntityId);

        [then]
        public void events_should_be_read_from_the_store()
        {
            _mockedEventStore.Verify(x => x.OpenStream(EntityId,0,int.MaxValue));
        }

        [then]
        public void the_result_should_be_the_entity_from_the_factory()
        {
            _result.ShouldEqual(_mockedEntity.Object);
        }

        [then]
        public void all_events_should_be_applied()
        {
            AppliedEvents.ShouldContainOnly(_eventMessages.Select(x => x.Body).ToList<object>());
        }

        [then]
        public void all_events_should_be_applied_in_order()
        {
            AppliedEvents.ShouldBeInTheSameOrderAs(_eventMessages.Select(x => x.Body).ToList<object>());
        }
    }
}