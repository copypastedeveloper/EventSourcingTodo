using Example.Todo.Domain.Common;

namespace Example.Data.EventStore
{
    public interface IEntityFactory
    {
        TEntity Create<TEntity>() where TEntity : IEntity;
    }
}