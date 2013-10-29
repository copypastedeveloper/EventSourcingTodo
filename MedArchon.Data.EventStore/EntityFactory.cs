using System;
using MedArchon.Todo.Domain.Common;

namespace MedArchon.Data.EventStore
{
    public class EntityFactory : IEntityFactory
    {
        public TEntity Create<TEntity>() where TEntity : IEntity
        {
            //todo add unit tests
            return (TEntity)Activator.CreateInstance(typeof(TEntity), true);
        }
    }
}