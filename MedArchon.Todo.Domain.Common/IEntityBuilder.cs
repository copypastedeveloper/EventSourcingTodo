using System;

namespace MedArchon.Todo.Domain.Common
{
    public interface IEntityBuilder
    {
        TEntity GetById<TEntity>(Guid id) where TEntity : IEntity;
        TEntity GetById<TEntity>(Guid id, int version) where TEntity : IEntity;
    }
}