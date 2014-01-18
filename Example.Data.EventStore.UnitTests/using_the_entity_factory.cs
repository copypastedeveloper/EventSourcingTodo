using System;
using System.Collections.Generic;
using Given.Common;
using Given.NUnit;
using Example.Todo.Domain.Common;

namespace Example.Data.EventStore.UnitTests
{
    public class using_the_entity_factory : Scenario
    {
        static EntityFactory _factory;
        static TestEntity _result;

        given an_entity_factory = () => _factory = new EntityFactory();

        when building_an_entity = () => _result = _factory.Create<TestEntity>();

        [then]
        public void the_entity_should_be_created()
        {
            _result.ShouldBeOfType<TestEntity>();
            _result.ShouldNotBeNull();
        }

        class TestEntity : IEntity
        {
            public Guid Id { get; set; }
            public void ApplyEvent(object @event)
            {
                throw new NotImplementedException();
            }

            public Queue<object> UncommittedEvents()
            {
                throw new NotImplementedException();
            }

            public void ClearUncommittedEvents()
            {
                throw new NotImplementedException();
            }
        }
    }
}